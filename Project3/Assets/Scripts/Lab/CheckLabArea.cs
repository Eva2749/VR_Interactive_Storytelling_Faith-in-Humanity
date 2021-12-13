using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.Events;


public class CheckLabArea : MonoBehaviour
{
    //public UnityEvent Enter_Event;
    public bool hasEntered;
    private bool CoroutineStart;
    public AudioSource sci_talk3;
    public AudioSource sci_talk4;
    public AudioSource sci_talk5;
    public AudioSource scientistCorrect1;
    public GameObject user;

    public GameObject teleportGrowing;

    public GameObject UI;

    //reference automove script
    public AutoMove automoveScript;

    public AudioSource response1;
    public AudioSource response2;
    public AudioSource sci_urge1;
    public AudioSource sci_confirm1;
    public AudioSource LeavingMonologue;
    public AudioClip LeavingMonologueClip;
    public AudioSource scientistLeaving;

    public bool teleportStartGrow;

    public CheckPouringArea checkPouringAreaScript;
    public GameObject bottle;
    public GameObject LabAnchorArea;
    XRGrabInteractable bottleGrab;


    //public GameObject response1Object;
    //public GameObject response2Object;
    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;
    public CheckPouring checkPouringScript;

    public LabUIController labUIcontrollerScript;
    public ParticleTrigger particleTriggerScript;

    private bool PickupRightBottle;

    private void Start()
    {
        //if (Enter_Event == null)
        //{
        //    Enter_Event = new UnityEvent();
        //}
        //response1Object.SetActive(false);
        //response2Object.SetActive(false);
        CoroutineStart = true;
        PickupRightBottle = true;
        teleportStartGrow = false;

        //get the grab interactable
        bottleGrab = bottle.GetComponent<XRGrabInteractable>();
        bottleGrab.enabled = !bottleGrab.enabled;
    }

    private void Update()
    {
        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            //exit the function only if find the user 
            if (user == null) return;
        }

        if (hasEntered && CoroutineStart)
        {
            automoveScript.sci_addition1.Stop();
            StartTalking();
            CoroutineStart = false;
        }

        if (particleTriggerScript.scientistUrge)
        {
            StartCoroutine(ScientistStartUrge());
            particleTriggerScript.scientistUrge = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("user"))
        //{
            hasEntered = true;
            
        //}
        //if (Enter_Event != null && !hasEntered)
        //{
        //    Enter_Event.Invoke();

        //}
    }

    public void StartTalking()
    {
        StartCoroutine(StartSpeaking());
    }

    IEnumerator StartSpeaking()
    {
        raycastControllerScript.DisableRaycast();
        directControllerScript.EnableDirect();
        yield return new WaitForSeconds(2);
        //when reaching the lab area, turn to the user
        automoveScript.LookSmoothly(automoveScript.user);

        automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);


        sci_talk3.Play();
        yield return new WaitForSeconds(sci_talk3.clip.length);

        sci_talk4.Play();
        yield return new WaitForSeconds(sci_talk4.clip.length);
        sci_talk5.Play();
        yield return new WaitForSeconds(sci_talk5.clip.length - 1);

        bottleGrab.enabled = enabled;

        automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);
        yield return new WaitForSeconds(1);

        //enable raycast
        directControllerScript.DisableDirect();
        raycastControllerScript.EnableRaycast();

    }

    public void PlayResponse1()
    {
        labUIcontrollerScript.UIappear = false;
        //set talking animation
        automoveScript.LookSmoothly(automoveScript.user);
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
        //correspondant audio
        response1.Play();
        StartCoroutine(StopResponding(response1));
    }

    public void PlayResponse2()
    {
        labUIcontrollerScript.UIappear = false;
        automoveScript.LookSmoothly(automoveScript.user);
        //set talking animation
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
        //correspondant audio
        response2.Play();
        StartCoroutine(StopResponding(response2));
    }

    //scientist finishes response to the user's response
    IEnumerator StopResponding(AudioSource response)
    {
        yield return new WaitForSeconds(response.clip.length);
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);

        yield return new WaitForSeconds(2);
        //start internal monologue
        LeavingMonologue.PlayOneShot(LeavingMonologueClip,1.0f);
        yield return new WaitForSeconds(LeavingMonologue.clip.length);

        automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
        scientistLeaving.Play();
        yield return new WaitForSeconds(scientistLeaving.clip.length);
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);

        teleportGrowing.SetActive(true);
        teleportStartGrow = true;
    }

    //scientist talk after the user sees the plant grow
    IEnumerator ScientistStartUrge()
    {
        automoveScript.LookSmoothly(user);
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
        sci_urge1.Play();
        yield return new WaitForSeconds(sci_urge1.clip.length);
        automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);

        yield return new WaitForSeconds(1);
        //the UI appears
        labUIcontrollerScript.UIappear = true;
        LabAnchorArea.SetActive(false);
    }

    public void ScientistConfirm1()
    {
        if (PickupRightBottle)
        {
            scientistCorrect1.Stop();
            sci_confirm1.Play();
            PickupRightBottle = false;
        }
    }

    public void StartScientistCorrect()
    {
        StartCoroutine(ScientistStartCorrect());
    }

    IEnumerator ScientistStartCorrect()
    {
        yield return new WaitForSeconds(5);

        while (!checkPouringAreaScript.allowPourEnter)
        {
            scientistCorrect1.Play();
            automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
            yield return new WaitForSeconds(scientistCorrect1.clip.length);
            automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);
            yield return new WaitForSeconds(5);
        }

    }
}
