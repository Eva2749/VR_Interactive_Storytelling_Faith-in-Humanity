using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;

    //duplicate the fill sliding value in the shader
    [Range(0, 2f)]
    public float fillAmount;

    [Range(0, 1f)]
    public float pouringSpeed;

    float angle;

    float currentFill = 0.386f;

    bool hasPouredOut = false;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        //pouringSpeed = 0.1f;
        //fillAmount = 1f;
        //StartPouring();
    }

    //public void StartPouring()
    //{
    //    StartCoroutine(PouringStart());
    //}

    private void Update()
    {
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;

        //update pouring angle
        //dot product: 1 if aligned together,-1 if opposite and 0 if perpendicular
        angle = Vector3.Dot(Vector3.up, transform.up);

        float curFill = Mathf.Clamp(Mathf.Lerp(0.5f, 0.386f, angle), 0.386f, 0.5f);


        if(angle < 0 && !hasPouredOut)
        {
            hasPouredOut = true;
            StartCoroutine(PouringStart(curFill));
            //Debug.Log("pouring out");
        }
        else if(!hasPouredOut)
        {
            rend.material.SetFloat("_Fill", curFill);
            //Debug.Log("not pouring out");
        }


        //Debug.Log("angle " + angle);
    }

    IEnumerator PouringStart(float fill)
    {
        //map from 1 to -1 to fill levels
        fillAmount = fill;

        while (fillAmount != 0)
        {
            //Debug.Log(fillAmount);
            fillAmount = Mathf.MoveTowards(fillAmount, 0, pouringSpeed * Time.deltaTime);
            rend.material.SetFloat("_Fill", fillAmount);
            //Debug.Log("pour level: " + fillAmount);
            //Debug.Log("angle" + angle);
            //Debug.Log("fillAmount" + fillAmount);
            yield return null;
        }

        //trigger event for pouring out finished, stop particles

    }

    public void PouringStop()
    {
        StopAllCoroutines();
    }

}
