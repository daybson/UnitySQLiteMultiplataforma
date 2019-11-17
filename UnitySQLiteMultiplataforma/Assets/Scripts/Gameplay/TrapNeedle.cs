using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNeedle : MonoBehaviour
{
    public Weapon weapon;

    void Start()
    {
        this.weapon = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(3);
        print(this.weapon.Name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("atacou personagem");
            other.GetComponent<CharacterController>().TakeDamage(this.weapon.Attack);
        }
    }

}
