using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchItem : MonoBehaviour
{
    public UnityEvent<Collider> OnTouchPlayer = new UnityEvent<Collider>();

    void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.TAG_PLAYER))
        {
            PlayAfterTouch(other);
        }
    }

    public virtual void Init()
    {
        
    }

    public virtual void PlayAfterTouch(Collider other)
    {
        OnTouchPlayer.Invoke(other);
    }
}
