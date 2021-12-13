using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    //used to control the trigger event only once
    bool hasNotTriggered;
    public GrowPlant growPlantScript;
    int particleCount = 0;

    public Material liquidMaterial;
    private Color liquidColor;

    private bool shiningStart;
    public float intensity;
    public float emissionSpeed;
    private bool stopTap;

    private bool tapClose;
    ParticleSystem tapSystem;
    public GameObject tapParticles;
    Vector3 tapPosition;
    Vector3 plantAreaPosition;

    public GameObject plantArea;
    Vector3 decreaseHeight = new Vector3(0.0f, 0.002f, 0.0f);

    public bool scientistUrge;

    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;

    public GameObject checkPouringArea;

    //https://docs.unity3d.com/Manual/PartSysTriggersModule.html

    private void Start()
    {


        tapSystem = GetComponent<ParticleSystem>();
        OnParticleTrigger();

        hasNotTriggered = true;
        stopTap = true;

        liquidMaterial.DisableKeyword("_EMISSION");

        //float parameters
        intensity = 0.0f;
        emissionSpeed = 0.2f;
        tapPosition = tapParticles.transform.position;
    }

    private void Update()
    {
        if (shiningStart)
        {
            //set liquid color
            liquidMaterial.EnableKeyword("_EMISSION");
            liquidColor = new Color(0.388f, 0.333f, 0.184f, 0.0f);
            liquidMaterial.SetColor("_EmissionColor", liquidColor);
            //make the liquid shine (emissive color)
            StartCoroutine(LiquidShining());
            shiningStart = false;
        }

        if (tapClose && tapPosition.y > plantArea.transform.position.y)
        {
            plantArea.transform.position += decreaseHeight;
            //StartCoroutine(PlantAreaDecrease());
            //tapClose = false;
        }
        else if (stopTap && tapPosition.y < plantArea.transform.position.y)
        {
            Vector3 plantAreaPosition;
            plantAreaPosition = plantArea.transform.position;
            plantAreaPosition.y = tapPosition.y;
            tapSystem.Stop();
            stopTap = false;
        }
    }

    void OnParticleTrigger()
    {

        //get particle system
        ParticleSystem ps = GetComponent<ParticleSystem>();
        //access particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get the entered particles
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        //int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        //count the number of particles entered
        particleCount += numEnter;

        //if particles entered reach number 10
        if (particleCount > 10 && hasNotTriggered)
        {
            //planting condition meet
            growPlantScript.startGrow = true;
            shiningStart = true;
            tapClose = true;
            //only for once
            hasNotTriggered = false;
        }
    }

    IEnumerator LiquidShining()
    {

        while (intensity < 1.5f)
        {
            //Debug.Log(intensity);
            intensity += emissionSpeed * Time.deltaTime;
            liquidMaterial.SetColor("_EmissionColor", liquidColor * intensity);
            yield return null;
        }
        //trigger scientist talking
        scientistUrge = true;
        checkPouringArea.SetActive(false);

        directControllerScript.DisableDirect();
        raycastControllerScript.EnableRaycast();
    }

    //IEnumerator PlantAreaDecrease()
    //{
    //    //when the plant area doesn't exceed the particle system
    //    while(tapPosition.y > plantAreaPosition.y)
    //    {
    //        plantAreaPosition += decreaseHeight;
    //        Debug.Log(plantAreaPosition);
    //        yield return null;
    //    }

    //    tapPosition.y = plantAreaPosition.y;

    //    Debug.Log("tap close");
    //}
}
