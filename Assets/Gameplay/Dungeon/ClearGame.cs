using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearGame : MonoBehaviour
{
    public DungeonTileEnd EndTile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.TAG_PLAYER))
        {
            EndTile.ClearGame();
        }
    }
}
