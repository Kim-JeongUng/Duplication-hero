using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  //�������̽�

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

    public void OnBeginDrag(PointerEventData eventData){    //�巡�װ� ���۵� �� �ѹ��� ����
        
    }

    public void OnDrag(PointerEventData eventData){         //�巡�� ���϶� ����
        rot.y += Input.GetAxis("Mouse X") * speed;
        character.transform.localEulerAngles = -rot;
    }

 

    public void OnEndDrag(PointerEventData eventData){      //�巡�װ� ���� �� �ѹ��� ����
        character.transform.localEulerAngles = origin;
        rot = origin;
    }


}
