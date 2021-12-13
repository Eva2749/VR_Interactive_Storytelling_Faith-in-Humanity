using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransporterController : MonoBehaviour
{
    public string destination;

    public bool canTransport = false;

    public void Transport()
    {
        if(canTransport)
        {
            XRSceneTransitionManager.Instance.TransitionTo(destination);
        }
    }
}
