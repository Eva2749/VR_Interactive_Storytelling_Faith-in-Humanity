using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPouringArea : MonoBehaviour
{
    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;
    public AudioSource ConfirmPouring;
    public CheckLabArea checkLabAreaScript;

    //public AudioSource scientistConfirm1;
    public bool allowPourEnter;

    private void OnTriggerEnter(Collider other)
    {
        checkLabAreaScript.scientistCorrect1.Stop();
        ConfirmPouring.Play();
        raycastControllerScript.DisableRaycast();
        directControllerScript.EnableDirect();
        //scientistConfirm1.Play();
        allowPourEnter = true;

    }

}
