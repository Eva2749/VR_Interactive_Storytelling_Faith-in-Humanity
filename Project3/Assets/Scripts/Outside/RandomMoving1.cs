using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMoving1 : MonoBehaviour
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

    bool getNewPath;

    //play characters' voice
    public AudioSource manScreaming;
    //private object newPos;

    void OnAnimatorMove()
    {
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
        agent.nextPosition = transform.position;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        animator = GetComponent<Animator>();

        isRunningHash = Animator.StringToHash("isRunning");
        isStandingHash = Animator.StringToHash("isStanding");
        //generate a new correct path initially
        getNewPath = true;
    }

    private void Update()
    {
        //switch animations based on randomness
        SwitchingAnimation();
        //set the running or standing status to true
        CallAnimation();

        if (getNewPath)
        {
            GenerateCorrectPath();
            //Debug.Log("generate a new path");
            getNewPath = false;
        }

        //Debug.Log(target);
    }

    private void SwitchingAnimation()
    {

        timer += Time.deltaTime;

        if (timer >= frequency)
        {
            rand_num = Random.Range(0, 100);

            //make standing if under the threshold
            if (rand_num < threshold)
            {
                if (!animator.GetBool(isStandingHash))
                {
                    //Debug.Log("set standing");
                    standing = true;
                    running = false;
                    animator.SetBool(isStandingHash, standing);
                    animator.SetBool(isRunningHash, running);
                }
            }
            //make running if above the threshold
            else
            {
                if (!animator.GetBool(isRunningHash)) {
                    //Debug.Log("set running");
                    running = true;
                    standing = false;
                    animator.SetBool(isRunningHash, running);
                    animator.SetBool(isStandingHash, standing);
                }
                else if (agent.remainingDistance - agent.stoppingDistance <= 2.5)
                {
                    //Debug.Log("reached destination! choose new location");
                    //StopCoroutine(StartScreaming());
                    getNewPath = true;
                    //running = true;
                    //standing = false;
                    //animator.SetBool(isRunningHash, running);
                    //animator.SetBool(isStandingHash, standing);
                }
            }

            timer = 0;
        }

    }

    private void CallAnimation()
    {
        //if the running state is true, make it run
        if (running)
        {
            //Debug.Log("started running");

            //StartCoroutine(StartScreaming());

            if (!animator.GetBool(isStandingHash))
            {
                agent.SetDestination(target);
            }

            manScreaming.Play();

            //https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent-isStopped.html
            //isStopped stop the nav mesh agent 
            agent.isStopped = false;
            running = false;
        }

        //if the standing state is true, stop his movement
        if (standing)
        {
            //Debug.Log("stopped running");
            getNewPath = false;
            StopAllCoroutines();
            agent.isStopped = true;
            manScreaming.Stop();
            //make sure coroutine only runs once
            standing = false;
        }
    }


    //generate a new random position inside the designated area
    Vector3 getNewRandomPosition()
    {
        float x = Random.Range(0.0f, 1.0f) * runningArea.bounds.size.x + runningArea.bounds.center.x - runningArea.bounds.size.x / 2.0f;
        float z = Random.Range(0.0f, 1.0f) * runningArea.bounds.size.z + runningArea.bounds.center.z - runningArea.bounds.size.z / 2.0f;

        Vector3 newPos = new Vector3(x, 0, z);
        return newPos;
    }

    void GetNewPath()
    {
        target = getNewRandomPosition();
    }

    //return the target value
    void GenerateCorrectPath()
    {
        GetNewPath();

        //check if the new path is invalid
        validPath = agent.CalculatePath(target, path);
        if (!validPath) Debug.Log("find an invalid path");

        //if get an invalid path, regenerate the path again
        while (!validPath)
        {
            GetNewPath();
            //update the bool again for continuous check
            validPath = agent.CalculatePath(target, path);
        }

        Vector3 runningManVector = runningMan.transform.position;

        //while (target.x - runningManVector.x <= 5 && target.y - runningManVector.y <= 5)
        //{
        //    GetNewPath();
        //}
    }

    //IEnumerator StartScreaming()
    //{
    //    while (true)
    //    {
    //        //play the audio every 5 seconds
    //        yield return new WaitForSeconds(10);
    //        manScreaming.Play();
    //        Debug.Log("man screaming");
    //    }
    //}


}
