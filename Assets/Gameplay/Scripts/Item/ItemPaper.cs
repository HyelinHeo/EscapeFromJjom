using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPaper : Item
{
    protected override void OnTouchItem(Collider col)
    {
        base.OnTouchItem(col);

        GuiPlaying.ShowPaper(true);
    }
}
