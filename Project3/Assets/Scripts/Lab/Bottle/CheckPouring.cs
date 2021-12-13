using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for triggering the robot hand particle system: hasTriggered bool
public class CheckPouring : MonoBehaviour
{
    public bool allowPour;
    public ParticleSystem tapLiquid;
    //public GameObject leftHandController;
    int particleCount = 0;
    public bool hasTriggered;
    bool playOnce;
    public bool allowUrge;
    public AudioSource bubbleSound;
    //public GameObject ChemicalDrop;
    public GameObject pump;
    public GameObject pump2;

    public ParticleSystem chemicalDrop;
    Vector3 pumpUp = new Vector3(0.0f, 0.01f, 0.0f);
    Vector3 pumpLeft = new Vector3(0.0f, 0.0f, 0.01f);
    Vector3 pumpPosition;
    Vector3 pump2Position;

    public AudioSource waterPumpSound;

    private void Start()
    {
        pump.SetActive(false);
        pump2.SetActive(false);
        allowUrge = true;
        OnParticleTrigger();
        hasTriggered = false;
        playOnce = true;
        //get particle system
        //chemicalDrop = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (hasTriggered)
        {
            //planting condition meet
            StartCoroutine(OpenTab());
            hasTriggered = false;
        }

        pumpPosition = pump.transform.position;
        pump2Position = pump2.transform.position;

    }

    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        //access particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get the entered particles
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        //int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        //count the number of particles entered
        particleCount += numEnter;

        if (particleCount > 3 && playOnce)
        {
            allowUrge = false;
            hasTriggered = true;
            playOnce = false;
        }
    }

    //for the bumping up and trigger robot hand particles
    IEnumerator OpenTab()
    {
        yield return new WaitForSeconds(2);
        //close the pouring particles first
        chemicalDrop.Stop();

        //Debug.Log("pouring in the right place now");
        //start the pump
        pump.SetActive(true);

        waterPumpSound.Play();

        while (pumpPosition.y <= 2.898f)
        {

            pump.transform.position += pumpUp;
            yield return null;
        }

        pump.SetActive(false);
        yield return new WaitForSeconds(0.006f);

        pump2.SetActive(true);

        while(pump2Position.z >= -3.563f)
        {
            Debug.Log(pump2.transform.position);

            pump2.transform.position -= pumpLeft;
            yield return null;
        }

        pump2.SetActive(false);


        yield return new WaitForSeconds(2);
        tapLiquid.Play();
        bubbleSound.Play();
    }

}
