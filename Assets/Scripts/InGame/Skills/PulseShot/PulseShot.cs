using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShot : MonoBehaviour
{
    public Entity Parent; //누가 쐈는지

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.parent.parent.GetComponent<ObjectMove>().Attacker;

        // 몬스터가 공격 시
        if (other.gameObject.CompareTag("Player") && Parent.CompareTag("Enemy"))  // 회오리가 캐릭터에 맞고 Attacker가 Enemy 이면
        {
            // 캐릭터에게 데미지 줌
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        // 캐릭터가 공격 시
        else if (other.gameObject.CompareTag("Enemy") && Parent.CompareTag("Player"))  // 회오리가 몬스터에 맞고 Attacker가 Player 이면
        {
            // 몬스터에게 데미지
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
}
