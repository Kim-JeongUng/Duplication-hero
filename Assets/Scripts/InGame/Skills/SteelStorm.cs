using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelStorm : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // 회오리가 캐릭터에 맞으면
        {
            // 캐릭터 공중에 띄움
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);

            // + 캐릭터에 데미지 추가
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
