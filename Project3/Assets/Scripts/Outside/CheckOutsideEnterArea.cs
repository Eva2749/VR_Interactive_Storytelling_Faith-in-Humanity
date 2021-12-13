using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutsideEnterArea : MonoBehaviour
{
    public GameObject cult1;
    public GameObject cult2;
    public GameObject cult3;

    public GameObject user;

    public bool hasEntered = false;

    public bool cult1Start;

    public CultController cultControllerScript;


    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;


    private void OnTriggerEnter(Collider other)
    {
        hasEntered = true;
        raycastControllerScript.DisableRaycast();
        directControllerScript.EnableDirect();
    }


    // Update is called once per frame
    void Update()
    {
        //make sure the XR is loaded into the scene
        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            //exit the function only if find the user 
            if (user == null) return;
        }

        if (hasEntered)
        {
            //start the cult1
            cult1Start = true;
            //cultControllerScript.CultStartWalking(cult1);
            //cultControllerScript.CheckDestination(cult1);
        }
    }
}
