using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GUIMenu : GUIWindow
{
    public GUIQuestionName GuiQuestionName;
    public GUIRecord GuiRecord;
    public GUISelectDungeon GuiSelectList;
    public OnClickMoveScene MoveScene;
    public GUIStory GuiStory;

    public Button BtnStart;
    public Button BtnLoad;
    public Button BtnRecord;
    void Start()
    {
        GuiQuestionName.Hide();
        GuiRecord.Hide();
        GuiStory.Hide();
        GuiSelectList.Hide();

        GuiRecord.OnChangedShow.AddListener(OnChangeRecord);
        GuiQuestionName.OnClickSaveName.AddListener(OnClickSaveName);
        BtnStart.onClick.AddListener(StartGame);
        BtnLoad.onClick.AddListener(LoadGame);
        BtnRecord.onClick.AddListener(ViewRecord);
    }

    private void OnLoadDungeonFile()
    {
        MoveScene.OnClick();
    }

    private void LoadGame()
    {
        GuiSelectList.Show();
        GuiSelectList.OnLoadFiled.AddListener(OnLoadDungeonFile);
    }

    private void OnChangeRecord(bool show)
    {
        if (!show)
        {
            Show();
        }
    }

    private void OnClickSaveName()
    {
        GuiQuestionName.Hide();
        //GuiStory.Show();
        MoveScene.OnClick();
    }

    void StartGame()
    {
        TileImporter.Inst.CurrentFile = null;

        string playerName = PlayerPrefs.GetString(PlayerPrefManager.STR_PLAYER_NAME, string.Empty);
        if (string.IsNullOrEmpty(playerName))
        {
            Hide();
            GuiQuestionName.Show();
        }
        else
        {
            MoveScene.OnClick();
            //GuiStory.Show();
        }
    }


    void ViewRecord()
    {
        Hide();
        GuiRecord.Show();
    }
}
