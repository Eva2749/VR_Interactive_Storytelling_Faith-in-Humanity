using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIcontroller : MonoBehaviour
{
    public static bool choice = false;
    public AudioSource MainCharacterVoiceLines;
    //public GameObject dialogueController;
    public GameObject otherButton; 
    
    // public static bool audio1done=false;
    // public  GameObject dialogueController; 
    // public Button button; 
    // public bool choiceMade = false; 

    void Start(){ 
        // dialogueController = GameObject.FindWithTag("Dialogue Controller"); 

        // dialogueController.SetActive(false); 
        Debug.Log("stupid start");
    }

    public void ChoiceMade(){
        Debug.Log("I have been summoned, your grace!");
        StartCoroutine(WaitTillEndofAudio());

      
       


    }

    // public void audio1isdone(){ 
    //     if(audio1done==true&&choice==false){ 
    //         dialogueController.SetActive(true);
    //     }
    // }

    //public void EndofAudioMC()
    //{
    //    StartCoroutine(WaitTillEndofAudio());
    //}

    IEnumerator WaitTillEndofAudio()
    {
        //dialogueController.SetActive(false);
        otherButton.SetActive(false);
        MainCharacterVoiceLines.Play();
        yield return new WaitForSeconds(MainCharacterVoiceLines.clip.length);
        choice = true;


    }


}
