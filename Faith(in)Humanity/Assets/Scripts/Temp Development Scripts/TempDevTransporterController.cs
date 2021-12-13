using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDevTransporterController : MonoBehaviour
{
    public string scene;

    public void Transition()
    {
        devXRSceneTransitionManager.Instance.TransitionTo(scene); 
    }

}

