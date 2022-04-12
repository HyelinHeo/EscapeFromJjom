using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIGameOver : GUIWindow, IPointerDownHandler
{
    public OnClickMoveScene MoveScene;

    void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveScene.OnClick();
    }
}
