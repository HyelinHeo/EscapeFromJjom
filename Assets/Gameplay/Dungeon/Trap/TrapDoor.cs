using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : Traps
{
    public override void Start()
    {
        base.Start();
    }

    public override IEnumerator OpenCloseTrap()
    {
        TrapAnim.SetTrigger("open");
        yield return new WaitForSeconds(2);
        TrapAnim.SetTrigger("close");
        yield return new WaitForSeconds(2);

        StartCoroutine(OpenCloseTrap());
    }
}
