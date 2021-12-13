using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 

public class Area1Controller : XRSceneController
{
    public Transform xrRigOrigin2;
    public XRBaseInteractable keyCard; 
    public XRSocketInteractor keyCardSocket;

    public override void Init()
    {
        Debug.Log(PlayerManager.Instance.hasVisitedArea2);
        if (PlayerManager.Instance.hasVisitedArea2)
        {
            keyCardSocket.startingSelectedInteractable = keyCard; 
        }
        Debug.Log(PlayerManager.Instance.hasVisitedArea2);
    }

    public override Transform GetXRRigOrigin()
    {
        Debug.Log(PlayerManager.Instance.hasVisitedArea2);
        return PlayerManager.Instance.hasVisitedArea2 ? xrRigOrigin2 : xrRigOrigin; 
    }

}
