using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YBotController : MonoBehaviour
{
    Animator animator;

    //create a hash so that no need to check for "isWalking" string every frame
    //a lot faster
    int isWalkingHash;
    int velocityxHash;
    int velocityzHash;

    NavMeshAgent agent;


    public void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        isWalkingHash = Animator.StringToHash("isWalking");
        velocityxHash = Animator.StringToHash("velocityX");
        velocityzHash = Animator.StringToHash("velocityZ");

    }

    public void Update()
    {
        //Vector3 velocity = agent.velocity;

        //check if the agent is moving and has not reached destination
        //https://docs.unity3d.com/540/Documentation/ScriptReference/NavMeshAgent-remainingDistance.html
        bool isMoving = agent.velocity.magnitude > 0.01f && agent.remainingDistance > agent.radius;
        animator.SetBool(isWalkingHash, isMoving);

        //turn velocity from worldspace to local
        //velocity = transform.InverseTransformVector(velocity);

        //animator.SetFloat(velocityxHash, velocity.x);
        //animator.SetFloat(velocityzHash, velocity.z);

        //animator.SetBool("isWalking", true);
        //transform.position += transform.forward * Time.deltaTime;
    }
}
