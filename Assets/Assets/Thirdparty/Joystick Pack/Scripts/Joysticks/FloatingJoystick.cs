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
        if(movePointer != null)
            movePointer.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (movePointer != null)
            movePointer.SetActive(true);
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (movePointer != null)
            movePointer.SetActive(false);
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}