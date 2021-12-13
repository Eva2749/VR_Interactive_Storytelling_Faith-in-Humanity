using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMoving : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    //public CheckRunningArea checkRunningAreaScript;
    public GameObject runningMan;
    public BoxCollider runningArea;

    //for finding new path on navmesh
    NavMeshPath path;
    bool validPath;
    public float timerForNewPath;
    Vector3 target;

    //bool to control animation status
    int isRunningHash;
    int isStandingHash;
    public bool standing;
    public bool running;

    //generate random switching between two animation states
    int rand_num;
    float timer;
    public float frequency;
    public int threshold;

    //play characters' voice
    public AudioSource manScreaming;
    //private object newPos;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        animator = GetComponent<Animator>();

        isRunningHash = Animator.StringToHash("isRunning");
        isStandingHash = Animator.StringToHash("isStanding");
    }

    private void Update()
    {
        //switch between running and standing
        SwitchingAnimation();

        //set the running or standing status to true
        CallAnimation();
    }

    private void SwitchingAnimation()
    {

        timer += Time.deltaTime;

        if (timer >= frequency)
        {
            rand_num = Random.Range(0, 100);

            if (rand_num < threshold)
            {
                if (!animator.GetBool(isStandingHash))
                {
                    Debug.Log("set standing");
                    standing = true;
                    running = false;
                    animator.SetBool(isStandingHash, standing);
                    animator.SetBool(isRunningHash, running);
                }
            }
            else
            {
                if (!animator.GetBool(isRunningHash)) {
                    Debug.Log("set running");
                    running = true;
                    standing = false;
                    animator.SetBool(isRunningHash, running);
                    animator.SetBool(isStandingHash, standing);
                }
                else if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    Debug.Log("reached destination! choose new location");
                    StopCoroutine(StartScreaming());
                    running = true;
                }
            }

            //Debug.Log(rand_num);

            timer = 0;
        }

    }

    private void CallAnimation()
    {
        //if the running state is true, make it run
        if (running)
        {
            Debug.Log("started running");
            //run at random position 
            StartCoroutine(StartMove());

            //start playing the looping audio
            StartCoroutine(StartScreaming());
            running = false;
        }

        //if the standing state is true, stop his movement
        if (standing)
        {

            Debug.Log("stopped running");

            StopAllCoroutines();
            agent.isStopped = true;
            manScreaming.Stop();
            standing = false;
        }
    }


    //generate a new random position inside the designated area
    Vector3 getNewRandomPosition()
    {
        //Vector3 backVector;

        //if (!checkRunningAreaScript.runBackwards)
      
        float x = Random.Range(0.0f, 1.0f) * runningArea.bounds.size.x + runningArea.bounds.center.x - runningArea.bounds.size.x / 2.0f;
        float z = Random.Range(0.0f, 1.0f) * runningArea.bounds.size.z + runningArea.bounds.center.z - runningArea.bounds.size.z / 2.0f;
       
        //else
        //{
        //    //make a transform.backward
        //    backVector = runningMan.transform.forward * -1;
        //    x = backVector.x * 2;
        //    z = backVector.z * 2;
        //    Debug.Log("RunningBackwards");
        //}

        Vector3 newPos = new Vector3(x, 0, z);
        return newPos;
    }



    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(timerForNewPath);
        //get a new vector
        GetNewPath();
        //https://docs.unity3d.com/ScriptReference/AI.NavMesh.CalculatePath.html
        //return true if path is valid
        validPath = agent.CalculatePath(target, path);
        if (!validPath) Debug.Log("find an invalid path");

        //if get an invalid path, regenerate the path again
        while (!validPath)
        {
            GetNewPath();
            //update the bool again for continuous check
            validPath = agent.CalculatePath(target, path);
        }

        //setdestination when the path is valid
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    void GetNewPath()
    {
        target = getNewRandomPosition();
    }

    IEnumerator StartScreaming()
    {
        while (true)
        {
            //play the audio every 5 seconds
            yield return new WaitForSeconds(5);
            manScreaming.Play();
            Debug.Log("man screaming");

        }
    }

}
