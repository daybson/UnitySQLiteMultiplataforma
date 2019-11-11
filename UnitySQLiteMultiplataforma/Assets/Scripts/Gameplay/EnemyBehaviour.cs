using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    protected Animator animator;
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float minDistanceFromTarget;
    protected float distanceFromTarget;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float targetOffsetStop;
    [SerializeField]
    protected float attackTimer;
    protected float attackTimerCount;
    protected Weapon Weapon;
    protected Character character;


    private void Awake()
    {
        this.animator = GetComponentInChildren<Animator>();
        this.character = GamesCodeDataSource.Instance.CharacterDAO.GetCharacter(2);
        this.Weapon = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(this.character.WeaponId);
    }

    void Update()
    {
        if (this.character.Health <= 0)
            return;

        var displacementToTarget = target.position - transform.position;
        this.distanceFromTarget = displacementToTarget.sqrMagnitude;

        var run = this.distanceFromTarget <= this.minDistanceFromTarget;

        this.animator.SetBool("run", run);
        transform.forward = displacementToTarget.normalized;

        if (run)
            transform.position = Vector3.MoveTowards(transform.position,
                                                    target.position + new Vector3(1, 0, 1) * targetOffsetStop,
                                                    this.speed * Time.deltaTime); 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            attackTimerCount += Time.deltaTime;
            if (attackTimerCount >= attackTimer)
            {
                attackTimerCount = 0;
                this.animator.SetBool("attack", true);
                other.gameObject.SendMessage("ReceiveDamage", this.Weapon.Attack);
                Invoke("CancelAttack", 1);
            }
        }
    }

    private void CancelAttack() => this.animator.SetBool("attack", false);
    private void CancelDamage() => this.animator.SetBool("damage", false);
    private void ScheduleInativation() => this.animator.enabled = false;

    public void ReceiveDamage(int value)
    {
        var remainDamage = value - this.character.Defense;

        if (remainDamage > 0)
            this.character.Health -= remainDamage;
        else
            return;

        this.animator.SetBool("damage", true);
        Invoke("CancelDamage", 1);

        print("Enemy Health: " + this.character.Health);

        if (this.character.Health <= 0)
            this.animator.SetBool("death", true); 
    }
}
