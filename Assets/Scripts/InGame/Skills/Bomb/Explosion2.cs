using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2 : MonoBehaviour
{
    public Entity Parent; //´©°¡ ½ú´ÂÁö

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.parent.GetComponent<Skills>().Attacker;

        other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        /*
        if (Parent.CompareTag("Enemy") && other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        else if (Parent.CompareTag("Player") && other.gameObject.CompareTag("Enemy"))
        {
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }*/
    }
}
