using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRSceneController : MonoBehaviour
{
    public Transform xrRigOrigin;

    public virtual void Init()
    {
      
    }

    public virtual Transform GetXRRigOrigin()
    {
        return xrRigOrigin; 
    }
}
