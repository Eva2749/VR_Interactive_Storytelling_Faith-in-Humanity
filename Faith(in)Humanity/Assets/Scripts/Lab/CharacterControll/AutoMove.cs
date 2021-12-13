// ClickToMove.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Events;

//[RequireComponent(typeof(AudioSource))]
public class AutoMove : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject user;

    public GameObject AllowPourArea;

    //public GameObject table;

    //where the scientist would head to 
    public GameObject labTable;
    public GameObject tableCenter;

    //public CheckEnterArea checkEnterArea;
    public bool allowMove;
    public bool allowTalk;
    public bool allowUrge;

    public Animator animator;
    public int isWalkingHash;
    public int isTalkingHash;

    public AudioSource sci_addition1;

    public GameObject XROrigin;

    Coroutine smoothMove = null;

    //place the npc in front of the user's viewpoint/near the table
    Vector3 face_vector;
    Vector3 lab_vector;

    //reference lab area script to check enter status
    public CheckLabArea checklabScript;

    //audio
    public AudioSource sci_talk1;
    public AudioSource sci_talk2;

    public ChangeToDirectController directControllerScript;
    public ChangeToRaycastController raycastControllerScript;

    //check if it reaches the position
    public bool CanReachPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isTalkingHash = Animator.StringToHash("isTalking");
        allowTalk = true;

        AllowPourArea.SetActive(false);
    }


    private void Update()
    {
        //make sure the XR is loaded into the scene
        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            //exit the function only if find the user 
            if (user == null) return;
        }


        //control walking: whenever the destination is set, start walking to the part
        bool isMoving = (agent.velocity.magnitude > 0.001f || agent.angularSpeed > 0.0001f) && agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool(isWalkingHash, isMoving);

        //let boolean value of allowMove equal to isEntering in the checkEnterArea script
        //allowMove = checkEnterArea.hasEntered;

        allowUrge = checklabScript.hasEntered;

        if(allowUrge && sci_addition1.isPlaying)
        {
            sci_addition1.Stop();
        }

        if (agent.remainingDistance == 0) return;

        //if the scientist's distance from the user is 0.5f, start talking
        if (agent.remainingDistance < 0.2)
        {
            if (allowTalk)
            {
                //make sure the npc is facing the user
                LookSmoothly(user);
                animator.SetBool(isTalkingHash, true);

                //trigger the audio
                StartCoroutine(StartTalking());

                //the chunk of code only runs once
                allowTalk = false;
            }

        }

        //Debug.Log(agent.remainingDistance);
    }


    public void StartWalkiing()
    {
        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            //exit the function only if find the user 
            if (user == null) return;
        }

        face_vector = XROrigin.transform.position + XROrigin.transform.forward * 2;
        agent.SetDestination(new Vector3(face_vector.x, 0, face_vector.z));
    }


    //this only runs once
    IEnumerator StartTalking()
    {
        //play the first audio
        sci_talk1.Play();
        yield return new WaitForSeconds(sci_talk1.clip.length);
        //after the audio 1 stops, start the second one
        sci_talk2.Play();

        yield return new WaitForSeconds(sci_talk2.clip.length - 1);

        //after the talk is over, stop talking and walking towards the table
        animator.SetBool(isTalkingHash, false);

        directControllerScript.DisableDirect();
        raycastControllerScript.EnableRaycast();
        //walking to the table
        //lab_vector = labTable.transform.position + labTable.transform.forward * 2;
        //agent.SetDestination(new Vector3(lab_vector.x, 0, lab_vector.z));

        //the table is an empty gameobject
        agent.SetDestination(labTable.transform.position);

        yield return new WaitForSeconds(0.1f);

        //check if the scientist has reached the table
        while (agent.remainingDistance >= agent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        LookSmoothly(tableCenter);

        //now scientist has reached the table
        //wait for 4 seconds
        yield return new WaitForSeconds(4);

        //if the user has not come over, turn over to the user and start talking
        while (!allowUrge)
        {
            //Debug.Log(checklabScript.hasEntered);
            LookSmoothly(user);
            animator.SetBool(isTalkingHash, true);

            //trigger the audio
            sci_addition1.Play();
            yield return new WaitForSeconds(sci_addition1.clip.length - 1.0f);
            //after the audio is played, stop the talking animation
            animator.SetBool(isTalkingHash, false);

            yield return new WaitForSeconds(4);
        }

    }

   
  

    //call the looksmoothly functions
    public void LookSmoothly(GameObject target)
    {
        float time = 1f;

        Vector3 lookAt = target.transform.position;
        lookAt.y = transform.position.y;

        //Start new look-at coroutine
        if (smoothMove == null)
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        else
        {
            //Stop old one then start new one
            StopCoroutine(smoothMove);
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        }
    }

    //call the coroutine to turn the head towards the user
    IEnumerator LookAtSmoothly(Transform objectToMove, Vector3 worldPosition, float duration)
    {
        Quaternion currentRot = objectToMove.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPosition - objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToMove.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }

}