using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponePlayer : MonoBehaviour
{
    public Transform ResponePoint;
    public Transform Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Player = ResponePoint;
        }
    }
}
