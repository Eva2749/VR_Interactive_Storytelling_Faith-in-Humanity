using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public AudioSource LastLine; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LetMeBreathe()); 
        StartCoroutine(GoodbyeBois());
        
    }

    // Update is called once per frame
    IEnumerator GoodbyeBois(){ 
        yield return new WaitForSeconds(30); 
        Application.Quit(); 

    }
    IEnumerator LetMeBreathe(){ 
        yield return new WaitForSeconds(6); 
        LastLine.Play(); 
        yield return new WaitForSeconds(LastLine.clip.length); 
        


    }

    
}
