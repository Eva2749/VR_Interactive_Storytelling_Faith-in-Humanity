using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardController : MonoBehaviour
{
    public TransporterController ctrl;
    public MeshRenderer buttonRenderer;

    public void CardInserted()
    {
        buttonRenderer.material.color = Color.green;
        ctrl.canTransport = true;
    }

    public void CardRemoved()
    {
        buttonRenderer.material.color = Color.red;
        ctrl.canTransport = false;
    }
}
