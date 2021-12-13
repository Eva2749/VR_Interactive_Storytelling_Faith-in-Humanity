using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//[AddComponentMenu("XR/Action Based Controller Manager")]
public class ChangeToRaycastController : MonoBehaviour
{
    //public GameObject DirectController;
    public GameObject RaycastController;

    //XRBaseInteractor TeleportInteractor;
    //XRBaseInteractor DirectInteractor;

    //private void Start()
    //{
    //    TeleportInteractor = LefthandController.GetComponent<XRRayInteractor>();
    //}
    private void Awake()
    {
        if (RaycastController == null)
        {
            RaycastController = GameObject.FindWithTag("RaycastController");
            //exit the function only if find the lefthand
            if (RaycastController == null) return;
        }
    }

    private void Update()
    {
        if (RaycastController == null)
        {
            RaycastController = GameObject.FindWithTag("RaycastController");
            //exit the function only if find the lefthand
            if (RaycastController == null) return;
        }

        //if (DirectController == null)
        //{
        //    DirectController = GameObject.FindWithTag("DirectController");
        //    //exit the function only if find the lefthand
        //    if (DirectController == null) return;
        //}

 
    }


    public void EnableRaycast()
    {
        RaycastController?.SetActive(true);
        //DirectController.SetActive(true);

        //TeleportInteractor.enabled = !TeleportInteractor.enabled;
        //DirectInteractor = LefthandController.GetComponent<XRDirectInteractor>();
      
        //if (DirectInteractor == null)
        //{
        //    XRBaseInteractor newDirectInteractor = LefthandController.AddComponent(typeof(XRDirectInteractor)) as XRDirectInteractor;
        //}
    }

    public void DisableRaycast()
    {
        RaycastController?.SetActive(false);
        //DirectController.SetActive(false);
        //DirectInteractor = LefthandController.GetComponent<XRDirectInteractor>();
        //DirectInteractor.enabled = !DirectInteractor.enabled;
        //if (TeleportInteractor == null)
        //{
        //    XRBaseInteractor newRayInteractor = LefthandController.AddComponent(typeof(XRRayInteractor)) as XRRayInteractor;
        //}
    }
}
