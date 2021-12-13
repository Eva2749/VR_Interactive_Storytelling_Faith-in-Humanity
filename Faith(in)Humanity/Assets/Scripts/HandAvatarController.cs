using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAvatarController : MonoBehaviour
{
    public Animator handAnimator;
    public InputActionReference gripInput;
    public InputActionReference pointInput;

    int gripAmount = Animator.StringToHash("GripAmount");
    int pointAmount = Animator.StringToHash("PointAmount");

    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
        handAnimator.keepAnimatorControllerStateOnDisable = true;
    }

    private void OnEnable()
    {
        gripInput.action.performed += OnGrip;
        pointInput.action.performed += OnPoint;
    }

    private void OnDisable()
    {
        gripInput.action.performed -= OnGrip;
        pointInput.action.performed -= OnPoint;
        handAnimator.SetFloat(gripAmount, 0);
        handAnimator.SetFloat(pointAmount, 0);
    }

    void OnGrip(InputAction.CallbackContext ctx)
    {
        float amt = ctx.ReadValue<float>();
        handAnimator.SetFloat(gripAmount, amt);
    }

    void OnPoint(InputAction.CallbackContext ctx)
    {
        float amt = ctx.ReadValue<float>();
        handAnimator.SetFloat(pointAmount, amt);
    }
}
