using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  //인터페이스

public class RotateCharacter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject character;
    float speed = 5f;
    Vector3 rot;
    Vector3 origin;
    void Start()
    {
        rot = character.transform.localEulerAngles;
        origin = rot;
    }

    public void OnBeginDrag(PointerEventData eventData){    //드래그가 시작될 때 한번만 실행
        
    }

    public void OnDrag(PointerEventData eventData){         //드래그 중일때 실행
        rot.y += Input.GetAxis("Mouse X") * speed;
        character.transform.localEulerAngles = -rot;
    }

 

    public void OnEndDrag(PointerEventData eventData){      //드래그가 끝날 때 한번만 실행
        character.transform.localEulerAngles = origin;
        rot = origin;
    }


}
