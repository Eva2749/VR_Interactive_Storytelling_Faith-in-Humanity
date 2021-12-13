using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckRunningArea : MonoBehaviour
{
    public bool runBackwards;

    private void OnTriggerEnter(Collider other)
    {
        runBackwards = false;
        Debug.Log("run forward");
    }


    private void OnTriggerExit(Collider other)
    {
        runBackwards = true;
        Debug.Log("run backward");
    }
}
