using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGame : Singleton<ControlGame>
{
    public CreatorTile Creator;
    public PlayerController PlayerCon;

    GUIPlaying GuiPlaying;

    void Start()
    {
        GuiPlaying = GUIManager.Inst.Get<GUIPlaying>();
        StartGame();
    }

    public void StartGame()
    {
        Creator.CreateTiles();
        PlayerCon.StartPlayer();
    }


    public void RestartGame()
    {
        Creator.InitPlayerPos();
        PlayerCon.StartPlayer();
        GuiPlaying.InitStart();
    }

}
