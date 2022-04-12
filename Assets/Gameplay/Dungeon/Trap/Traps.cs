using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public Animator TrapAnim;
    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(OpenCloseTrap());
    }


   public virtual IEnumerator OpenCloseTrap()
    {
        return null;
    }
}
