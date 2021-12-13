using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.XR.Interaction.Toolkit;

public class HolographManController : MonoBehaviour
{
    // Animator animator; 
    // NavMeshAgent agent; 
    public GameObject Government; 
    public GameObject dialogueController; 

    public AudioSource myAudio1;
    public AudioSource myAudio2;
    public static bool ButtonPressedOnce =false;
    public GameObject Teleporter;
    
    public Animator myAnimation;
    //public UIcontroller Control;
    //public UIcontroller Control2;

    public int isTalkingHash;
    public int isStandingHash;


    void Start(){ 
        // dialogueController = GameObject.FindWithTag("Dialogue Controller"); 
        Debug.Log("I am here");
        // dialogueController.SetActive(false);
        Government = GameObject.FindWithTag("GovOfficial"); 
        myAnimation = GetComponent<Animator>();


        // m= GetComponent<AudioSource>(); 
        isStandingHash = Animator.StringToHash("isStanding");
        isTalkingHash = Animator.StringToHash("isTalking");
        myAnimation.SetBool(isTalkingHash, true);

    }

    //Playing the government message and turning the government official off 
    //once the thing is done playing

    void Update(){ 
        

        // if (UIcontroller.choice ==false&& dialogueController ==null){ 
        //     Debug.Log("Long if statements");
        //     //  dialogueController = GameObject.FindWithTag("Dialogue Controller"); 
               
        // }
        // if(!myAudio1.isPlaying){ 
        //     // Government.SetActive(false);
        //     // Government.enabled = false
        //     myAnimation.GetComponent<Animator>().enabled=false;
        //     UIcontroller.dialogueController.SetActive(true);
        //     // Debug.Log("")
                
        // } 


        if (UIcontroller.choice ==true&&!myAudio2.isPlaying){
            //UIcontroller.MainCharacterVoiceLines.Play();
            //Control.EndofAudioMC();
            //Control2.EndofAudioMC();
            if (!myAudio2.isPlaying){
                myAudio2.Play(); 
                Debug.Log("I am happening.");
                dialogueController.SetActive(false);
                //talking
                myAnimation.SetBool(isStandingHash, false);
                myAnimation.SetBool(isTalkingHash, true);
                //myAnimation.GetComponent<Animator>().enabled =true; 
                StartCoroutine(WaitForEndOfAudio2()); 
                

            }
            

        }
        
    

    }

      

    IEnumerator WaitForEndOfAudio1(){
        yield return new WaitForSeconds(myAudio1.clip.length);
        //myAnimation.GetComponent<Animator>().enabled = false;
        myAnimation.SetBool(isTalkingHash, false);
        myAnimation.SetBool(isStandingHash, true);

        dialogueController.SetActive(true);
        // myAnimation.SetBool(isStandingHash,true);
        // myAnimation.SetBool(isStandingHash, true);

        // dialogueController = GameObject.FindWithTag("Dialogue Controller"); 
        
        Debug.Log("DialogueController set to trUU");
        

        // UIcontroller.audio1done=true;
        // Government.SetActive(false); 
    }

    IEnumerator WaitForEndOfAudio2(){ 
        yield return new WaitForSeconds(myAudio2.clip.length);
        myAnimation.SetBool(isTalkingHash, false);

        yield return new WaitForSeconds(1);
        //myAnimation.GetComponent<Animator>().enabled =false; 
        Government.SetActive(false); 
        dialogueController.SetActive(false);
        TeleportGuide.messageSequenceDone = true;
        Teleporter.SetActive(true);
       
    }

    // private void Update()
    // { 

    //     // animator.SetBool(isPlayingMessageHash, isPlaying);

    // }

    public void StartMyCoroutine1() {
        ButtonPressedOnce = true; 
        StartCoroutine(WaitForEndOfAudio1()); 
    }

}
