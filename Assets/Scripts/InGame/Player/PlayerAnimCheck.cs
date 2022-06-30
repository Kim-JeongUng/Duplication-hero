using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCheck : MonoBehaviour
{
    public GameObject Weapon;
    private void Awake()
    {
        if (Weapon == null)
        {
            Debug.Log("Weapon Null");
        }
    }
    // Start is called before the first frame update

    //Slah �ִϸ��̼� �� ������ ����� �� �ݶ��̴� ����
    public void isSlashStart() => Weapon.GetComponent<BoxCollider>().enabled = true;
    public void isSlashStop() => Weapon.GetComponent<BoxCollider>().enabled = false;
}
