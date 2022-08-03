using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelStorm : MonoBehaviour
{
    public Entity Parent; //���� ������

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // ȸ������ ĳ���Ϳ� ������
        {
            // ĳ���� ���߿� ���
            StartCoroutine(UpPlayer(other));

            // ĳ���Ϳ��� ������ ��
            Parent = transform.parent.GetComponent<ObjectMove>().Attacker;
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
    IEnumerator UpPlayer(Collider other)
    {
        // ĳ���� ���� ����
        other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);

        // ĳ���� �������̰� Position X,Y ������
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(1f);

        // 1�� �� ������ ���� �� Rotation X,Z �� �ٽ� ���
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
