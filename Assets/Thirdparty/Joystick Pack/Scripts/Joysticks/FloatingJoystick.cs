using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : Joystick
{
    

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
        movePointer.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        movePointer.SetActive(true);
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        movePointer.SetActive(false);
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}