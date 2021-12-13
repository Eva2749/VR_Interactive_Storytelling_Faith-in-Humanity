using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIFINAL : MonoBehaviour
{    public string scene; 

        public static bool finalchoice = false; 
    // Start is called before the first frame update
      void Start(){ 
        // dialogueController = GameObject.FindWithTag("Dialogue Controller"); 

        // dialogueController.SetActive(false); 
        Debug.Log("stupid start");
    }

    public void ChoiceMade1(){ 
        Debug.Log("I have been summoned, your grace!"); 
        finalchoice = true;
        devXRSceneTransitionManager.Instance.TransitionTo(scene); 


    }

    public void ChoiceMade2(){ 
        Debug.Log("I have been summoned, your grace!"); 
        finalchoice = true;
        devXRSceneTransitionManager.Instance.TransitionTo(scene); 



    }
}


