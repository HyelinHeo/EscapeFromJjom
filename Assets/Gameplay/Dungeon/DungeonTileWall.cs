using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTileWall : MonoBehaviour
{
    public MeshRenderer Ren;
    public Collider Col;

    void Start()
    {
        
    }

    public virtual void Show()
    {
        Ren.enabled = true;
        Col.enabled = true;
    }

    public virtual void Hide()
    {
        Ren.enabled = false;
        Col.enabled = false;
    }
}
