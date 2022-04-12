using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Singleton<PlayerController>
{
    public CreatorTile Creatortile;
    public PlayerActionController PlayerPrefab;

    public PlayerActionController CurrentPlayer;

    public UnityEvent SpawnPlayer = new UnityEvent();

    public bool WayTest;

    void Start()
    {
    }

    public void StartPlayer()
    {
        if (CurrentPlayer != null)
            DestroyImmediate(CurrentPlayer.gameObject);

        CurrentPlayer = Instantiate(PlayerPrefab);
        CurrentPlayer.transform.position = Creatortile.PlayerPos;

        if (WayTest)
        {

        }

        SpawnPlayer.Invoke();
        Debug.Log("SpawnPlayer");
    }
 
}
