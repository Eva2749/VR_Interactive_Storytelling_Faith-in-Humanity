using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    //variables for pouring interaction
    public int pourThreshold = 60;
    public bool isPouring = false;
    bool pouringCheck;
    public AudioSource scientistCorrect1;
    public CheckPouring checkPouringScript;
    public AutoMove automoveScript;

    // test for tree grow
    public bool treegrow = false;

    //public GrowPlant growPlantScript;
    public CorkDetector corkdetectorScript;

    ParticleSystem waterDropSystem;
    Coroutine pourCorrect = null;

    private void Awake()
    {
        waterDropSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        //bool pourCheck = CalculatePourAngle() < pourThreshold;
        bool pourCheck = CheckForAngle();
        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring && corkdetectorScript.corkOut)
            {
                //Debug.Log("isPouring = true");
                StartPour();
                if (pourCorrect == null)
                {
                    //Debug.Log("pourcorrect not null");

                    pourCorrect = StartCoroutine(CheckPouringRight());
                }
            }
            else
            {
                EndPour();
                if (pourCorrect != null)
                {
                    StopCoroutine(pourCorrect);
                    pourCorrect = null;
                }
            }
        }

        if(checkPouringScript.hasTriggered && pourCorrect != null)
        {
            if(scientistCorrect1.isPlaying) scientistCorrect1.Stop();
            StopCoroutine(pourCorrect);
            pourCorrect = null;
        }
    }

    private void StartPour()
    {
        waterDropSystem.Play();

    }

    private void EndPour()
    {
        //Debug.Log("Not Pouring");
        waterDropSystem.Stop();
    }

    private bool CheckForAngle()
    {
        //the dot function sets a vector that compares with the project's vector
        return Vector3.Dot(Vector3.down, transform.up) > 0;
    }

    IEnumerator CheckPouringRight()
    {
        //Debug.Log("checkpouringright start");

        yield return new WaitForSeconds(2);

        while (isPouring && !checkPouringScript.hasTriggered && checkPouringScript.allowUrge)
        {
            //Debug.Log("scientist reminder");

            scientistCorrect1.Play();
            automoveScript.animator.SetBool(automoveScript.isTalkingHash, true);
            yield return new WaitForSeconds(scientistCorrect1.clip.length);
            automoveScript.animator.SetBool(automoveScript.isTalkingHash, false);

            yield return new WaitForSeconds(8);
        }
        pourCorrect = null;
    }

}
