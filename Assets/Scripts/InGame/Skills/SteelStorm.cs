using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelStorm : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // ȸ������ ĳ���Ϳ� ������
        {
            // ĳ���� ���߿� ���
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000f, ForceMode.Force);

            // + ĳ���Ϳ� ������ �߰�
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
