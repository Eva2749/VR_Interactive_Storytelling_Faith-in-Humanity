using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class LinearFixtureSliderUpdateEvent : UnityEvent<float>
{}

public class LinearFixtureInteractable : XRBaseInteractable
{
    [Header("Linear Fixture")]

    public Transform start;
    public Transform end;
    public Transform slider;

    public LinearFixtureSliderUpdateEvent sliderUpdate = new LinearFixtureSliderUpdateEvent();

    XRBaseInteractor interactor;
    Coroutine sliderCoroutine;

    //returns percentage distance of v between a and b
    float InverseLerp(Vector3 a, Vector3 b, Vector3 v)
    {
        Vector3 a2b = b - a;
        Vector3 a2v = v - a;
        //project a to v on to a to b divide by length^2 of a to b
        return Mathf.Clamp01(Vector3.Dot(a2v, a2b) / Vector3.Dot(a2b, a2b));
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (interactor == null)
        {
            interactor = args.interactor;
            StartDrag();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactor == interactor)
        {
            EndDrag();
            interactor = null;
        }
        base.OnSelectExited(args);
    }


    void StartDrag()
    {
        if (sliderCoroutine != null) StopCoroutine(sliderCoroutine);
        sliderCoroutine = StartCoroutine(CalculateDrag());
    }

    void EndDrag()
    {
        if (sliderCoroutine != null) StopCoroutine(sliderCoroutine);
        sliderCoroutine = null;
    }

    IEnumerator CalculateDrag()
    {
        //run this coroutine while the interactor is in use
        while (interactor != null)
        {
            //line from start to end
            Vector3 line = start.position - end.position;

            //orthogonally project interactor position onto line, set it to slider pos
            Vector3 orthoProjection = Vector3.Project(interactor.transform.position, line.normalized);

            //inverse lerp the new position between the start and end to get the percentage
            float percent = InverseLerp(start.position, end.position, orthoProjection);

            //same as ortho projection, but clamped to start and end
            slider.position = Vector3.Lerp(start.position, end.position, percent);

            //update the event if any listeners
            sliderUpdate?.Invoke(percent);

            yield return null; //yield control back, so we pick up the next loop iteration in the next frame
        }
    }
}

#if UNITY_EDITOR
public class LinearFixtureInteractableGizmoDrawer
{
    static Vector3 SLIDER_SIZE = new Vector3(0.1f, 0.1f, 0.1f);

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoForMyScript(LinearFixtureInteractable src, GizmoType gizmoType)
    {
        Color current = Gizmos.color;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(src.start.position, src.end.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(src.start.position, 0.05f);
        Gizmos.DrawWireSphere(src.end.position, 0.05f);

        Gizmos.color = src.isSelected ? Color.red : Color.yellow;

        Gizmos.DrawWireCube(src.slider.position, SLIDER_SIZE);

        Gizmos.color = current;
    }
}

#endif