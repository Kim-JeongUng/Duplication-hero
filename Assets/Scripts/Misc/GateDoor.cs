using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoor : MonoBehaviour
{
    public bool Ldoor, Rdoor;
    bool isFixDoor = false;
    void Awake()
    {
        Ldoor = false;
        Rdoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Ldoor){
            this.transform.Rotate(new Vector3(0, 60f, 0) * Time.deltaTime);
        }
        else if(Rdoor){
            this.transform.Rotate(new Vector3(0, -60f, 0) * Time.deltaTime);
        }
        if((Ldoor || Rdoor) && !isFixDoor){
            Invoke("FixDoor", 2.4f);
            isFixDoor = true;
        }
    }

    void FixDoor(){
        if(Ldoor)
            this.transform.rotation = Quaternion.Euler(0, -40f, 0);
        else if(Rdoor)
            this.transform.rotation = Quaternion.Euler(0, -320f, 0);
        Ldoor = false;
        Rdoor = false;
    }
}
