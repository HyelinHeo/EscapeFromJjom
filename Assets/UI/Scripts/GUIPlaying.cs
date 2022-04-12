using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlaying : GUIWindow
{
    public GUIPlayer Player;
    public GUIPlayer NPC_Woman;

    public GUIMessageBox MessageBox;
    public GUITimer GuiTimer;
    public GUIBattery GuiBattery;

    public OnClickMoveScene MoveScene;

    private InputGamePlay myInput;

    public PlayerActionController PlayerAction;
    private GUIQuestionDungeonName GuiQuestionName;


    public Image ImgPaper;

    public override void Awake()
    {
        base.Awake();
        myInput = new InputGamePlay();
        myInput.Game.Exit.performed += Escape_performed;
    }

    public void InitStart()
    {
        Player.Init();
        GuiTimer.ResetTimer();
        GuiTimer.Play();
    }

    private void OnEnable()
    {
        myInput.Enable();
    }

    private void OnDisable()
    {
        myInput.Disable();
    }

    void Start()
    {
        MessageBox = GUIManager.Inst.Get<GUIMessageBox>();
        GuiQuestionName = GUIManager.Inst.Get<GUIQuestionDungeonName>();

        string playerName = PlayerPrefs.GetString(PlayerPrefManager.STR_PLAYER_NAME);
        Player.SetPlayerName(playerName);
        NPC_Woman.SetPlayerName("유미");
        NPC_Woman.Hide();

        ShowPaper(false);

        GuiTimer.Play();
        GuiBattery.Hide();
        //GuiBattery.Show();
        //GuiBattery.OnChangedValue.AddListener(OnChangeValueBattery);

        PlayerController.Inst.SpawnPlayer.AddListener(SpawnPlayer);
    }

    private void SpawnPlayer()
    {
        PlayerAction = PlayerController.Inst.CurrentPlayer;
        PlayerAction.OnRun.AddListener(RunStartPlayer);
        PlayerAction.OnRunCancel.AddListener(RunEndPlayer);
    }

    private void RunEndPlayer()
    {
        Debug.Log("StopDecrease");
        Player.GuiStamina.StopDecrease();
        Player.GuiStamina.RunIncrease();
    }

    private void RunStartPlayer()
    {
        Player.GuiStamina.StopIncrease();
        Player.GuiStamina.RunDecrease();
    }

    public void ShowPaper(bool show)
    {
        if (ImgPaper.enabled != show)
            ImgPaper.enabled = show;
    }

    private void OnChangeValueBattery(float value)
    {
        if (value <= 0)
        {
            Player.SetStatus(PlayerStatus.FEAR);
        }
        else
        {
            Player.SetStatus(PlayerStatus.NORMAL);
        }
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!MessageBox.isShow)
        {
            MessageBox.Show(StrManager.EXIT_GAME_QUESTION, GUIMessageBox.MessageButtons.YESNO);
            MessageBox.ResultYes.AddListener(() => StartCoroutine(ExitGame()));
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }


    public IEnumerator ExitGame()
    {
        yield return null;
        MessageBox.Show(StrManager.SAVE_DUNGEON_QUESTION, GUIMessageBox.MessageButtons.YESNO);
        MessageBox.ResultYes.AddListener(ShowQuestion);
        MessageBox.ResultNo.AddListener(MoveScene.OnClick);
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
}
