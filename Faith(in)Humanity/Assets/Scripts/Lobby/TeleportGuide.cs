using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGuide : MonoBehaviour
{
    public AudioSource TeleportGuideMessage;

    public static bool messageSequenceDone = false;
    public static bool hasGoneToScene2 = false; 
    public static bool hasEntered = false; 
    public static bool inTeleporter = false; 
    // Start is called before the first frame update
    void Start()
    {
    //   StartCoroutine(TeleportTime()); 
        
    }

    void Awake()
    {
        if (messageSequenceDone ==true)
        {
            if(!TeleportGuideMessage.isPlaying){ 
                StartCoroutine(TeleportTime());
            }
            
            // TeleportGuideMessage.SetActive(true); 
 
        }
        
    }

    IEnumerator TeleportTime()
    {
        Debug.Log("I am in the Coroutine and Teleport Guide Should Play, Maybe?");
        TeleportGuideMessage.Play();
        yield return new WaitForSeconds(4); 


    }
}
