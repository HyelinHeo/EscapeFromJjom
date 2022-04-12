using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GUIPlaying GuiPlaying;

    public TouchItem Touch;

    public string ID;
    public ItemType Itemtype;

    /// <summary>
    /// 필드 아이템 여부
    /// </summary>
    public bool IsFieldItem;

    [SerializeField]
    private MeshRenderer[] renders;

    public enum ItemType
    {
        Battery = 0,
        PAPER = 1
    }

    public static int MaxNo;
    public static int NewID()
    {
        return MaxNo++;
    }

    public virtual void Start()
    {
        NewID();
        if (renders == null || renders.Length == 0)
            renders = GetComponentsInChildren<MeshRenderer>();

        GuiPlaying = GUIManager.Inst.Get<GUIPlaying>();

        Touch.Init();
        Touch.OnTouchPlayer.AddListener(OnTouchItem);
    }

    protected virtual void OnTouchItem(Collider col)
    {
        SetActiveRenderer(false);
    }

    void SetActiveRenderer(bool enable)
    {
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].enabled = enable;
        }
    }


}
