using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIStory : GUIWindow, IPointerClickHandler
{
    public Text TxtStory;
    public OnClickMoveScene MoveScene;

    public void OnPointerClick(PointerEventData eventData)
    {
        MoveScene.OnClick();
    }

    public override void Show()
    {
        TxtStory.text = string.Format("수학 여행을 떠난 {0}...", PlayerPrefs.GetString(PlayerPrefManager.STR_PLAYER_NAME));

        base.Show();
    }

}
