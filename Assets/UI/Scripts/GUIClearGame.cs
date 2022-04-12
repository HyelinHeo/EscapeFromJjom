using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GUIClearGame : GUIWindow
{
    private GUIMessageBox Msg;
    private GUIQuestionDungeonName GuiQuestionName;

    public OnClickMoveScene MoveScene;
    public Text TxtRecord;

    private GUITimer GuiTimer;

    PlayerRecrodList recordList;

    void Start()
    {
        Msg = GUIManager.Inst.Get<GUIMessageBox>();
        GuiQuestionName = GUIManager.Inst.Get<GUIQuestionDungeonName>();
    }

    public void RestartGame()
    {
        ControlGame.Inst.RestartGame();
        Hide();
    }

    public override void Show()
    {
        base.Show();

        GuiTimer = GUIManager.Inst.Get<GUIPlaying>().GuiTimer;

        recordList = FileManager.Inst.Load<PlayerRecrodList>("record");
        if (recordList == null)
            recordList = new PlayerRecrodList();

        TxtRecord.text = GuiTimer.TimeRecord;

        string playerName = PlayerPrefs.GetString(PlayerPrefManager.STR_PLAYER_NAME);
        recordList.Add(playerName, GuiTimer.GetTime());

        string error = string.Empty;
        if (FileManager.Inst.Save(recordList, "record", ref error))
        {
            Debug.Log("Save Record OK");
        }
    }

    public void GoHome()
    {
        Msg.Show(StrManager.SAVE_DUNGEON_QUESTION, GUIMessageBox.MessageButtons.YESNO);
        Msg.ResultYes.AddListener(ShowQuestion);
        Msg.ResultNo.AddListener(MoveScene.OnClick);
    }

    void ShowQuestion()
    {
        GuiQuestionName.Show();
        GuiQuestionName.OnClickSaveName.AddListener(dungeonName => SaveGameOK(dungeonName));
    }

    private void SaveGameOK(string dungeonName)
    {
        TileExporter.Inst.Save(dungeonName);
        MoveScene.OnClick();
    }

    public void SetRecrod(string txt)
    {
        TxtRecord.text = txt;
    }
}
