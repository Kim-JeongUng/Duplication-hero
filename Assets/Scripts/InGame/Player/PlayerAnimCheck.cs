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

    //Slah 애니메이션 중 때리는 모션일 때 콜라이더 적용
    public void isSlashStart() => Weapon.GetComponent<BoxCollider>().enabled = true;
    public void isSlashStop() => Weapon.GetComponent<BoxCollider>().enabled = false;
}
