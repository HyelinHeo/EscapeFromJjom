using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCallback : MonoBehaviour
{
    public Animator PlayerAnim;

    void Hit()
    {
        Debug.Log("Hit!");
    }

    void FootR()
    {

    }

    void FootL()
    {

    }

    void Land()
    {
        //PlayerAnim.SetBool("JumpTrigger", false);
        Debug.Log("land");
    }
}
