using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float turn;
    protected float angleFacing;

    protected Animator animator;

    public float attackTimer;
    public float attackTimerCount;

    protected Weapon Weapon;
    protected Character character;


    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.character = GamesCodeDataSource.Instance.CharacterDAO.GetCharacter(1);
        this.Weapon = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(this.character.WeaponId);
    }


    void Update()
    {
        var direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(direction * this.speed * Time.deltaTime);

        this.animator.SetFloat("speed", direction.sqrMagnitude);

        angleFacing += Input.GetAxisRaw("Mouse X");
        transform.eulerAngles = new Vector3(0, angleFacing * turn, 0);

        this.animator.SetBool("attack", Input.GetMouseButtonDown(0));
    }


    public void ReceiveDamage(int value)
    {
        this.character.Health -= value - this.character.Defense;
        //print("Player Health: " + this.character.Health);
        GamesCodeDataSource.Instance.CharacterDAO.UpdateCharacter(this.character);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            attackTimerCount += Time.deltaTime;

            if (this.animator.GetBool("attack"))
            {
                if (attackTimerCount >= attackTimer)
                {
                    attackTimerCount = 0;
                    other.gameObject.SendMessage("ReceiveDamage", this.Weapon.Attack);
                }
            }
        }
    }
}
