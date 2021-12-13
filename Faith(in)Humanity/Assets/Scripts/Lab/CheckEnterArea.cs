using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckEnterArea : MonoBehaviour
{
    //public UnityEvent Enter_Event;
    
    public bool hasEntered = false;
    public GameObject EnterArea;
    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;

    BoxCollider b;

    public AutoMove autoMoveScript;

    //void Start()
    //{
    //    if(Enter_Event == null)
    //    {
    //        Enter_Event = new UnityEvent();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
       // if (other.gameObject.CompareTag("user"))
        //{
            //if (Enter_Event != null && !hasEntered)
            //{
            //Enter_Event.Invoke();
            hasEntered = true;
            
            autoMoveScript.StartWalkiing();
            raycastControllerScript.DisableRaycast();
            directControllerScript.EnableDirect();
            
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        EnterArea.SetActive(false);
    }
}
