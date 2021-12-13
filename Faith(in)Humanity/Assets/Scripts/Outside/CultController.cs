using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CultController : MonoBehaviour
{
    //public NavMeshAgent agent;
    public GameObject user;

    int isTalkingHash;
    int isPrayingHash;
    int isRunningHash;
    int isStandingHash;

    public GameObject cult1;
    public GameObject cult2;
    public GameObject cult3;

    public GameObject cult1Position;
    public GameObject cult2Position;
    public GameObject cult3Position;

    public AudioSource cultTalk1;
    public AudioSource cultTalk2;
    public AudioSource cultTalk3;
    public AudioSource cultTalk4;

    bool allowCultTalk1;
    bool allowCultTalk3;
    bool allowCultTalk2;

    bool cultwalk;

    //bool to contro
    bool allowCult1Walk;
    bool allowCult2Walk;
    bool allowCult3Walk;

    bool isMoving;



    int cultTalkingTimes = 0;
    int cult2FrameCount = 150;
    int cult3FrameCount = 320;

    Coroutine lookAt1 = null;
    Coroutine lookAt2 = null;
    Coroutine lookAt3 = null;

    Vector3 face_vector;

    public CheckOutsideEnterArea checkareaScript;
    int frameOffset = 0;
    int currentFrame = 0;

    public GameObject enterArea;
    public GameObject XROrigin;


    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;

    public AudioSource leavingMonologue;
    public GameObject teleportPortal;

    public bool teleportStartGlow;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        isRunningHash = Animator.StringToHash("isRunning");
        isTalkingHash = Animator.StringToHash("isTalking");
        isPrayingHash = Animator.StringToHash("isPraying");
        isStandingHash = Animator.StringToHash("isStanding");

        allowCultTalk1 = true;
        allowCultTalk2 = true;
        allowCultTalk3 = true;

        allowCult1Walk = true;
        allowCult2Walk = true;
        allowCult3Walk = true;

        cultwalk = true;
        teleportStartGlow = false;
    }

    private void Start()
    {
        frameOffset = Time.frameCount;   
    }


    void Update()
    {
        currentFrame = Time.frameCount - frameOffset;

        //Debug.Log(allowCult1Walk);

        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            if (user == null) return;
        }

        if (checkareaScript.cult1Start && allowCult1Walk)
        {
            CultStartWalking(cult1,new Vector3(0,0,0));
            CheckDestination(cult1);
        }

        if (currentFrame >= cult2FrameCount)
        {
            if(currentFrame == cult2FrameCount)
            {
                cultwalk = true;
            }

            if (allowCult2Walk)
            {
                CultStartWalking(cult2, XROrigin.transform.right * 2.0f);
                CheckDestination(cult2);
            }

        }



        if (currentFrame >= cult3FrameCount)
        {
            if (currentFrame == cult3FrameCount)
            {
                cultwalk = true;
            }

            if (allowCult3Walk)
            {
                CultStartWalking(cult3, XROrigin.transform.right * -2.0f);
                CheckDestination(cult3);
            }
        }

        if (cultTalkingTimes == 4)
        {
            StartCoroutine(GoBackToOriginal(cult1));
            StartCoroutine(GoBackToOriginal(cult2));
            StartCoroutine(GoBackToOriginal(cult3));

            StopCoroutine(lookAt1);
            StopCoroutine(lookAt2);
            StopCoroutine(lookAt3);

            enterArea.SetActive(false);

            cultTalkingTimes = 5;
        }
        //Debug.Log(Time.frameCount);

    }

    IEnumerator GoBackToOriginal(GameObject cult)
    {
        NavMeshAgent agent = cult.GetComponent<NavMeshAgent>();
        Animator animator = cult.GetComponent<Animator>();


        if (cult == cult1)
        {
            agent.SetDestination(cult1Position.transform.position);

            yield return new WaitForSeconds(0.001f);
            animator.SetBool(isStandingHash, false);

            while (agent.remainingDistance >= agent.stoppingDistance)
            {
                CultStartWalking(cult, new Vector3(0, 0, 0));
                yield return null;
            }

            animator.SetBool(isRunningHash, false);
            animator.SetBool(isPrayingHash, true);

        }

        if (cult == cult2)
        {
            yield return new WaitForSeconds(3);
            agent.SetDestination(cult2Position.transform.position);

            yield return new WaitForSeconds(0.001f);
            animator.SetBool(isStandingHash, false);

            while (agent.remainingDistance >= agent.stoppingDistance)
            {
                CultStartWalking(cult, new Vector3(0, 0, 0));
                yield return null;
            }

            animator.SetBool(isRunningHash, false);
            animator.SetBool(isPrayingHash, true);
        }

        if (cult == cult3)
        {
            yield return new WaitForSeconds(6);
            agent.SetDestination(cult3Position.transform.position);

            yield return new WaitForSeconds(0.001f);
            animator.SetBool(isStandingHash, false);

            while (agent.remainingDistance >= agent.stoppingDistance)
            {
                CultStartWalking(cult, new Vector3(0, 0, 0));
                yield return null;
            }

            animator.SetBool(isRunningHash, false);
            animator.SetBool(isPrayingHash, true);

            StartCoroutine(TransitionToFinal());
        }


    }


    IEnumerator TransitionToFinal()
    {
        yield return new WaitForSeconds(2);
        leavingMonologue.Play();
        yield return new WaitForSeconds(leavingMonologue.clip.length - 2);
        //activate teleport
        teleportPortal.SetActive(true);
        teleportStartGlow = true;
        //enable raycast controller 
        raycastControllerScript.EnableRaycast();
        directControllerScript.DisableDirect();

        //internal monologue

    }

    //public void StartWalkingAgain(GameObject cult,)
    //{
    //    isMoving = (agent.velocity.magnitude > 0.001f || agent.angularSpeed > 0.01f) && agent.remainingDistance > agent.stoppingDistance;
    //    animator.SetBool(isRunningHash, isMoving);
    //}

    public void CultStartWalking(GameObject cult, Vector3 facing_vector)
    {
        NavMeshAgent agent = cult.GetComponent<NavMeshAgent>();
        Animator animator = cult.GetComponent<Animator>();

        isMoving = (agent.velocity.magnitude > 0.001f || agent.angularSpeed > 0.01f) && agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool(isRunningHash, isMoving);

        //go to the user's destination
        if (cultwalk)
        {
            face_vector = XROrigin.transform.position + XROrigin.transform.forward * 3.0f;
            Vector3 to_user_vector = new Vector3(face_vector.x, 0, face_vector.z);
            agent.SetDestination(to_user_vector + facing_vector);
            cultwalk = false;
        }

    }
 


    //make it in the update mode to check continously
    public void CheckDestination(GameObject cult)
    {
        NavMeshAgent agent = cult.GetComponent<NavMeshAgent>();
        //Debug.Log(agent.remainingDistance);
        //Debug.Log(Time.frameCount);
        //if (agent.remainingDistance == 0) return;

        //when the cult reaches destination
        if (cult == cult1 && agent.remainingDistance < 0.8 && currentFrame >= 3 || cult == cult2 && agent.remainingDistance < 0.8 && currentFrame >= (cult2FrameCount+3) || cult == cult3 && agent.remainingDistance < 0.8 && currentFrame >= (cult3FrameCount+3))
        {

            if (cult == cult1 && allowCultTalk1)
            {
                lookAt1 = LookSmoothly(user, cult);

                StartCoroutine(CultStartTalking(cult));
                allowCultTalk1 = false;  //only talks once
                allowCult1Walk = false;  //to stop the checkDestination function
            }
            else if(cult == cult2 && allowCultTalk2)
            {
                lookAt2 = LookSmoothly(user, cult);
                Animator animator = cult.GetComponent<Animator>();
                animator.SetBool(isStandingHash, true);
                animator.SetBool(isRunningHash, false);
                allowCultTalk2 = false;  //to stop the checkDestination function
                allowCult2Walk = false;
            }
            else if(cult == cult3 && allowCultTalk3)
            {
                lookAt3 = LookSmoothly(user, cult);
                Animator animator = cult.GetComponent<Animator>();
                animator.SetBool(isStandingHash, true);
                animator.SetBool(isRunningHash, false);
                allowCultTalk3 = false;
                allowCult3Walk = false;    //to stop the checkDestination function
            }
        }
    }


    IEnumerator CultStartTalking(GameObject cult)
    {
        //make sure the npc is facing the user
        Animator animator = cult.GetComponent<Animator>();

        //make the current cult only talk
        animator.SetBool(isStandingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isTalkingHash, true);


        //if the first cult stops talking, make the second cult start talking
        if (cult == cult1 && cultTalkingTimes < 3)
        {
            cultTalkingTimes += 1;
            cultTalk1.Play();
            yield return new WaitForSeconds(cultTalk1.clip.length);

            //make the cult stop talking and stands
            animator.SetBool(isTalkingHash, false);
            animator.SetBool(isStandingHash, true);

            StartCoroutine(CultStartTalking(cult2));

        }
        else if(cult == cult2 && cultTalkingTimes < 3)
        {
            cultTalkingTimes += 1;
            cultTalk2.Play();
            yield return new WaitForSeconds(cultTalk2.clip.length);


            //make the cult stop talking and stands
            animator.SetBool(isTalkingHash, false);
            animator.SetBool(isStandingHash, true);

            StartCoroutine(CultStartTalking(cult3));
        }
        else if (cult == cult3 && cultTalkingTimes < 3)
        {
            cultTalkingTimes += 1;
            cultTalk3.Play();
            yield return new WaitForSeconds(cultTalk3.clip.length);

            //make the cult stop talking and stands
            animator.SetBool(isTalkingHash, false);
            animator.SetBool(isStandingHash, true);

            StartCoroutine(CultStartTalking(cult1));
        }
        else if (cult == cult1 && cultTalkingTimes == 3)
        {
            cultTalk4.Play();
            yield return new WaitForSeconds(cultTalk4.clip.length);

            //make the cult stop talking and stands
            animator.SetBool(isTalkingHash, false);
            animator.SetBool(isStandingHash, true);

            cultTalkingTimes += 1;
        }

    }



    //call the looksmoothly functions
    public Coroutine LookSmoothly(GameObject target, GameObject cult)
    {
        float time = 1f;

        //Vector3 lookAt = target.transform.position;
        //lookAt.y = cult.transform.position.y;

        //Start new look-at coroutine
        //if (smoothMove == null)
         return StartCoroutine(LookAtSmoothly(cult.transform, target.transform, time));
        //else
        //{
        //    //Stop old one then start new one
        //    StopCoroutine(smoothMove);
        //    smoothMove = StartCoroutine(LookAtSmoothly(cult.transform, lookAt, time));
        //}
    }

    //call the coroutine to turn the head towards the user
    IEnumerator LookAtSmoothly(Transform objectToMove, Transform target, float duration)
    {
        //Quaternion currentRot = objectToMove.rotation;
        //Quaternion newRot = Quaternion.LookRotation(worldPosition - objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float turnSpeed = 3.0f;

        while (true)
        {
            Vector3 newDir = Vector3.RotateTowards(objectToMove.forward, target.position - objectToMove.position, turnSpeed * Time.deltaTime, 0.0f);
            objectToMove.rotation = Quaternion.LookRotation(newDir);
            yield return null; 
        }
    }

}
