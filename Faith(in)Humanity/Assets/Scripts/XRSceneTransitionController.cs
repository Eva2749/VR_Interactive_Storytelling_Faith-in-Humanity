using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRSceneTransitionController : MonoBehaviour
{
    public string scene;

    public void TransitionScene()
    {
        XRSceneTransitionManager.Instance.TransitionTo(scene);
    }
}
