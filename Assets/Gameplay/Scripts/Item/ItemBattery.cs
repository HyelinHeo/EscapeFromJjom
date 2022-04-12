using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattery : Item
{
    private GUIBattery guiBattery;

    [Range(0f, 100f)]
    public float Capacity;
    public float DecreaseSpeed = 1f;

    public override void Start()
    {
        base.Start();

        guiBattery = GuiPlaying.GuiBattery;
    }

    protected override void OnTouchItem(Collider col)
    {
        base.OnTouchItem(col);

        guiBattery.ChargeBattery(Capacity / 100);
    }

    private void Update()
    {
        if (!IsFieldItem)
        {
            if (Capacity > 0f)
            {
                Capacity -= DecreaseSpeed * Time.deltaTime;
                if (Capacity < 0)
                    Capacity = 0;
            }
        }

    }
}
