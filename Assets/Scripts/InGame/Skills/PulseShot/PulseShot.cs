using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShot : MonoBehaviour
{
    public Entity Parent; //���� ������

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.parent.parent.GetComponent<ObjectMove>().Attacker;

        // ���Ͱ� ���� ��
        if (other.gameObject.CompareTag("Player") && Parent.CompareTag("Enemy"))  // ȸ������ ĳ���Ϳ� �°� Attacker�� Enemy �̸�
        {
            // ĳ���Ϳ��� ������ ��
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        // ĳ���Ͱ� ���� ��
        else if (other.gameObject.CompareTag("Enemy") && Parent.CompareTag("Player"))  // ȸ������ ���Ϳ� �°� Attacker�� Player �̸�
        {
            // ���Ϳ��� ������
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
}
