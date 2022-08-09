using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2 : MonoBehaviour
{
    public Entity Parent; //´©°¡ ½ú´ÂÁö

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.GetComponent<Explosion>().Attacker;

        if (other.gameObject.CompareTag("Player") && Parent.CompareTag("Enemy"))
        {
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        else if (other.gameObject.CompareTag("Enemy") && Parent.CompareTag("Player"))
        {
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
}
