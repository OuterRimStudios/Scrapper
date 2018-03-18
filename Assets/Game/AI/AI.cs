using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public AIReferenceManager refManager;

    public Transform chaseTarget;
    public float baseSpeed;
    public bool targetFriendlies;
    public bool updateTarget;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public bool attacking;
    [HideInInspector] public Vector3 walkPos;
    [HideInInspector] public Vector3 previousPos;
    [HideInInspector] public float speed;

    WaitForSeconds attackFrequency;

    public float updateTargetFrequency;                                      //How often the AI will update it's state
    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingTarget;                                                     //Checks if it's time to update the state
    bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes

    Rigidbody rb;
    [HideInInspector] public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        walkPos = transform.position;
        previousPos = transform.position;
        aiActive = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackFrequency = new WaitForSeconds(1);
        waitTime = new WaitForSeconds(updateTargetFrequency);

        speed = baseSpeed;
    }

    void Update()
    {
        if (!aiActive) return;

        if (!updateTarget) return;
        if (!updatingTarget)
        {
            updatingTarget = true;
            StartCoroutine(UpdateTarget());
        }
    }

    public void Move()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    public void SetDestination(Vector3 position)
    {
        walkPos = new Vector3(position.x, transform.position.y, position.z);
    }

    public void SetDestination(Transform target)
    {
        if(target)
            walkPos = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

    IEnumerator UpdateTarget()
    {
        if (refManager.exclusionTags.Length <= 0)
        {
            if (!targetFriendlies)
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString());
            else
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.friendlyTag.ToString());
        }
        else
        {
            if (!targetFriendlies)
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString(), refManager.exclusionTags.ToList());
            else
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.friendlyTag.ToString(), refManager.exclusionTags.ToList());
        }

        SetDestination(chaseTarget);
        yield return waitTime;
        updatingTarget = false;
    }

    public IEnumerator Attack()
    {
        yield return attackFrequency;
        attacking = false;
    }

    public void InteruptAgent()
    {
        if(agent)
        agent.destination = transform.position;
        refManager.aiAbility.DeactivateAbility();
        StopAgent();
    }

    public void StopAgent()
    {
        refManager.stateController.aiActive = false; 
        aiActive = false;
        if(agent)
        agent.isStopped = true;
    }

    public void StartAgent()
    {
        aiActive = true;
        refManager.stateController.currentState.CheckTransitions(refManager.stateController);
        refManager.stateController.aiActive = true;

        if(agent)
        agent.isStopped = false;
    }
}
