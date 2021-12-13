using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[System.Serializable]
public class RotationFixtureRotateUpdateEvent : UnityEvent<float>
{ }

public class RotationFixtureInteractable : XRBaseInteractable
{
    public RotationFixtureRotateUpdateEvent rotateUpdate = new RotationFixtureRotateUpdateEvent();

    [HideInInspector]
    public float angle;

    Vector3 startAngle = Vector3.zero;

    XRBaseInteractor interactor;
    Coroutine rotationCoroutine;

    //convert a world rotation to the local rotaion of this object
    Quaternion GetLocalRotation(Quaternion world)
    {
        return Quaternion.Inverse(world) * transform.localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (interactor == null)
        {
            interactor = args.interactor;
            StartRotation();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactor == interactor)
        {
            EndRotation();
            interactor = null;
        }
        base.OnSelectExited(args);
    }


    void StartRotation()
    {
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        startAngle = GetLocalRotation(interactor.transform.localRotation).eulerAngles;
        rotationCoroutine = StartCoroutine(CalculateRotation());
    }

    void EndRotation()
    {
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        startAngle = Vector3.zero;
        rotationCoroutine = null;
    }

    IEnumerator CalculateRotation()
    {
        //run this coroutine while the interactor is in use
        while (interactor != null)
        {
            //the current rotation angle is the delta from where we started to where we are now
            angle = GetLocalRotation(interactor.transform.rotation).eulerAngles.z - startAngle.z;
            
            rotateUpdate?.Invoke(angle);

            yield return null; //yield control back, so we pick up the next loop iteration in the next frame
        }
    }
}
