using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteelStorm : MonoBehaviour
{
    public Entity Parent; //���� ������

    private void OnTriggerEnter(Collider other)
    {
        Parent = transform.parent.GetComponent<ObjectMove>().Attacker;
        
        // ���Ͱ� ���� ��
        if (other.gameObject.CompareTag("Player") && Parent.CompareTag("Enemy"))  // ȸ������ ĳ���Ϳ� �°� Attacker�� Enemy �̸�
        {
            // ĳ���� ���߿� ���
            StartCoroutine(UpPlayer(other));

            // ĳ���Ϳ��� ������ ��
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
        // ĳ���Ͱ� ���� ��
        else if (other.gameObject.CompareTag("Enemy") && Parent.CompareTag("Player"))  // ȸ������ ���Ϳ� �°� Attacker�� Player �̸�
        {
            // ���� ���߿� ���
            StartCoroutine(UpPlayer(other));
            
            // ���Ϳ��� ������
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
    IEnumerator UpPlayer(Collider other)
    {
        Debug.Log(other.name + " move UP");

        // ĳ���� ���� ���
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);
        // ���� ���
        else
        {
            // ������ ������̼� ��Ȱ��ȭ
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 100f, ForceMode.Force);
        }

        // �������̰� Position X,Y ������
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(1f);

        if(other.gameObject.CompareTag("Enemy"))
            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        // 1�� �� ������ ���� �� Rotation X,Z �� �ٽ� ���
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    
    }
}
