using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonControllerFinal : XRBaseInteractable
{
    [Header("Button Fixture")]

    public Transform plunger;
    public float depressionDepth = 0.05f; //amount to move down along local y axis
    public float triggerThreshold = 0.005f; //the distance from the bottom that the button triggers
    //Animation
    public GameObject Government; 
    //Button Audio 
    public AudioSource Notification; 
    
    public UnityEvent buttonPressed = new UnityEvent();

    public HologramControllerFinal holoman;
    // cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
    public static bool wasPressed = false;
    float yStart = 0;
    float yOffset = 0;
    XRDirectInteractor interactor;
    Coroutine buttonPressCoroutine;
    // Government = GameObject.FindWithTag("GovOfficial");
    // Government.SetActive(false); 
    //cannot select the button, only hover interaction

    void Start(){ 
    // public soundDelay = 3.0; 
        // Government.SetActive(false); 
        Notification = GetComponent<AudioSource>(); 
        Notification.loop = true;
    // yield WaitForSeconds(soundDelay); 
    // Notification.Play();

    }

    // void Update(){ 
    //     while(wasPressed == false){ 
    //         yield retun new WaitForSeconds()
    //         Notification.loop = true;

    //     }
    // }
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        return false;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (interactor == null)
        {
            interactor = (XRDirectInteractor)args.interactor;
            StartPress();
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (args.interactor == interactor)
        {
            EndPress();
            interactor = null;
            yOffset = 0;
        }
        base.OnHoverExited(args);
    }


    void StartPress()
    {
        if (buttonPressCoroutine != null) StopCoroutine(buttonPressCoroutine);

        //capture starting position
        yStart = plunger.localPosition.y;

        //get interactor position in local space of the button's parent
        //so the y axis is always up and down the axis of the plunger
        Vector3 localInteractorPos = plunger.parent.InverseTransformPoint(interactor.transform.position);

        //set the initial offset from where the interactor enters the button space 
        yOffset = localInteractorPos.y - yStart;

        buttonPressCoroutine = StartCoroutine(CalculatePress());
    }

    void EndPress()
    {
        StopCoroutine(buttonPressCoroutine);
        buttonPressCoroutine = null;
        Vector3 localPos = plunger.localPosition;
        localPos.y = yStart;
        plunger.localPosition = localPos;
        wasPressed = false;
    }

    IEnumerator CalculatePress()
    {
        //run this coroutine while the interactor is in use
        while (interactor != null)
        {
            //get interactor position in local space of the button's parent
            //so the y axis is always up and down the axis of the plunger
            Vector3 localInteractorPos = plunger.parent.InverseTransformPoint(interactor.transform.position);

            //tget plunger local pos
            Vector3 localPos = plunger.localPosition;

            //set the plunger's local y to the interactors y minus the initial offset, limited to the allowable range of motion
            localPos.y = Mathf.Clamp(localInteractorPos.y - yOffset, yStart - depressionDepth, yStart);

            //assign the local position to move the plunger
            plunger.localPosition = localPos;

            

            //check if we've crossed the trigger threshold and trigger event
            if(!wasPressed && localPos.y < yStart - depressionDepth + triggerThreshold)
            {
                buttonPressed?.Invoke();
                wasPressed = true;
                Notification.loop = false;
                Notification.Stop();
                Debug.Log("we're alive");
                HologramControllerFinal.FinalMessageButtonPressed=true; 
                // HologramControllerFinal.
                holoman.PlayMyCoroutines();
                

                
                // set
            }

            yield return null; //yield control back, so we pick up the next loop iteration in the next frame
        }
    }

}
