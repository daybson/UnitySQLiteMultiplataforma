using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float WalkSpeed;
    public float TurnSpeed;
    protected Animator animator;

    protected Character character;
    protected Weapon weapon;

    public Slider LiferBar;
    protected TouchController touchController;

    void Start()
    {
        this.touchController = FindObjectOfType<TouchController>();

        this.character = GamesCodeDataSource.Instance.CharacterDAO.GetCharacter(1);
        this.weapon = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(this.character.WeaponId);

        this.LiferBar.value = this.character.Health;
        this.WalkSpeed = this.character.Agility;

        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        var movement = new Vector3(this.touchController.Direction.x, 0, this.touchController.Direction.y);
#else
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
#endif
        if (movement.x != 0 || movement.z != 0)
        {
            transform.Translate(movement * WalkSpeed * Time.deltaTime);
            this.animator.SetBool("speed", true);
        }
        else
            this.animator.SetBool("speed", false);

        var mouseX = Input.GetAxis("Mouse X");
        if (mouseX != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                              transform.eulerAngles.y + mouseX * TurnSpeed * Time.deltaTime,
                                              transform.eulerAngles.z);
        }

        var mouseClick0 = Input.GetMouseButtonDown(0);
        if (mouseClick0)
        {
            this.animator.SetBool("attack", true);
        }
        else
        {
            this.animator.SetBool("attack", false);
        }
    }


    public void TakeDamage(int damage)
    {
        var diff = damage - this.character.Defense;
        if (diff > 0)
        {
            this.character.Health -= diff;
            this.LiferBar.value = this.character.Health;
            GamesCodeDataSource.Instance.CharacterDAO.UpdateCharacter(this.character);

            if (this.character.Health <= 0)
                print("Death");
        }
    }

    public void IncreaseHealth(int bonus)
    {
        if (bonus > 0)
        {
            this.character.Health += bonus;
            this.LiferBar.value = this.character.Health;
            GamesCodeDataSource.Instance.CharacterDAO.UpdateCharacter(this.character);
        }
    }
}
