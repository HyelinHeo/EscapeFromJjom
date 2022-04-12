using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTileWallDoor : DungeonTileWall
{
    public MeshRenderer RenDoor;
    public Collider ColDoor;

    void Start()
    {

    }

    public override void Show()
    {
        base.Show();
        RenDoor.enabled = false;
        ColDoor.enabled = false;

    }

    public override void Hide()
    {
        base.Hide();

        RenDoor.enabled = true;
        ColDoor.enabled = true;
    }

}
