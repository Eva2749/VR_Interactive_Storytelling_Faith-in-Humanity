using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public float rotateSpeed = 0.2f;
    //also change controller


    // Update is called once per frame
    void Update()
    {
        rotateSpeed += 1.1f * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", rotateSpeed);
        //Debug.Log(RenderSettings.skybox.GetFloat("_Rotation"));
    }
}
