using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTileEnd : DungeonTile
{
    public void ClearGame()
    {
        GUIManager.Inst.Get<GUIClearGame>().Show();
    }
}
