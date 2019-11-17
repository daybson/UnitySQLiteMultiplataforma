using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHealth : MonoBehaviour
{
    public int HealthBonus;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {            
            other.GetComponent<CharacterController>().IncreaseHealth(HealthBonus);
            Destroy(gameObject);
        }
    }
}
