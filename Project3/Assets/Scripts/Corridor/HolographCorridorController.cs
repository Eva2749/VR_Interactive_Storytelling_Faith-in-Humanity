using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolographCorridorController : MonoBehaviour
{
    public GameObject mySelf;
    public GameObject nextSelf; 
    public GameObject Holograph;
    public GameObject nextHolograph;
    public GameObject Anchor;
    //public GameObject otherHolograph2;
    //public GameObject otherHolograph3;

    public AudioSource McTalk;
    public AudioClip McTalkk; 
    public void OnTriggerEnter(Collider other)
    {
        // Holograph = 
        if(other.gameObject.tag=="Right Hand")    
        {
            Anchor.SetActive(false);
            Debug.Log("AAAAAAAAAAAAAAA I AM COLLIDING!!");
            McTalk.PlayOneShot(McTalkk, 0.7F);
            StartCoroutine(SetThingsRight());


        }


        //if (!McTalk.isPlaying)
        //{
        //    StartCoroutine(SetThingsRight());
        //    Holograph.SetActive(false);
        //    otherHolograph1.SetActive(true);

        //}
        // else if (!other.gameObject.CompareTag("Holograph"))
        // { 
        //      Holograph.SetActive(true);
        // }


    }

      IEnumerator SetThingsRight()
    {
        yield return new WaitForSeconds(McTalk.clip.length); 
        yield return new WaitForSeconds(15);
        Debug.Log("I am in the coroutine");
        Holograph.SetActive(false);
        //nextHolograph.SetActive(true);
        mySelf.SetActive(false);
        nextSelf.SetActive(true);
        nextHolograph.SetActive(true);


    }
}


