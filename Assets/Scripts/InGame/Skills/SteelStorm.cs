using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelStorm : MonoBehaviour
{
    public Entity Parent; //누가 쐈는지

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // 회오리가 캐릭터에 맞으면
        {
            // 캐릭터 공중에 띄움
            StartCoroutine(UpPlayer(other));

            // 캐릭터에게 데미지 줌
            Parent = transform.parent.GetComponent<ObjectMove>().Attacker;
            other.transform.GetComponent<Entity>().TakeDamage(new DamageReport(Parent.Damage, Parent));
        }
    }
    IEnumerator UpPlayer(Collider other)
    {
        // 캐릭터 위로 띄우고
        other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);

        // 캐릭터 못움직이게 Position X,Y 프리즈
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(1f);

        // 1초 뒤 프리즈 해제 및 Rotation X,Z 만 다시 잠금
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
