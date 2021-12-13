using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramControllerFinal : MonoBehaviour
{
    public GameObject Government; 
    public GameObject Cult; 
    public GameObject Scientist; 

    public GameObject DialogueController; 

    public AudioSource GovernmentAudio; 
    public AudioSource CultAudio;
    public AudioSource ScientistAudio;
    public static bool FinalMessageButtonPressed; 
    // public bool GovermentSpoke; 
    // public bool CultSpoke; 
    // public bool ScientistSpoke;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(FinalMessages()); 
        
    }


    public void PlayMyCoroutines(){ 
        StartCoroutine(GovFinalMessage());
        // StartCoroutine(GovFinalMessage()); 
    }

    // Update is called once per frame
    IEnumerator CultFinalMessage(){ 
        Cult.SetActive(true); 
        CultAudio.Play(); 
        yield return new WaitForSeconds(CultAudio.clip.length); 
        yield return new WaitForSeconds(1); 
        Cult.SetActive(false);
        DialogueController.SetActive(true);


    }

    IEnumerator GovFinalMessage(){ 
        Government.SetActive(true); 
        GovernmentAudio.Play(); 
        yield return new WaitForSeconds(GovernmentAudio.clip.length); 
        yield return new WaitForSeconds(1); 
        Government.SetActive(false); 
        StartCoroutine(ScientistFinalMessage()); 

    }

    IEnumerator ScientistFinalMessage(){ 
        Scientist.SetActive(true); 
        ScientistAudio.Play(); 
        yield return new WaitForSeconds(ScientistAudio.clip.length); 
        yield return new WaitForSeconds(1); 
        Scientist.SetActive(false);
        StartCoroutine(CultFinalMessage());

    }


}
