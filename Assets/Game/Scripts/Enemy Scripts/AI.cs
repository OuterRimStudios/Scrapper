using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public AIReferenceManager refManager;

    public Transform chaseTarget;
    public bool targetFriendlies;
    public bool updateTarget;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public bool attacking;
    [HideInInspector] public Vector3 walkPos;
    [HideInInspector] public Vector3 previousPos;
    [HideInInspector] public float speed;
    public float updateTargetFrequency;                                      //How often the AI will update it's state
    public GameObject selfDestructEffect;
    
    WaitForSeconds attackFrequency;

    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingTarget;                                                     //Checks if it's time to update the state
    bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes
    bool selfDestructing;

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

        if(agent)
        speed = agent.speed;
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

    //public void Move()
    //{
    //    rb.velocity = transform.forward * speed * Time.deltaTime;
    //}

    public void SetDestination(Vector3 position)
    {
        walkPos = new Vector3(position.x, transform.position.y, position.z);
        agent.isStopped = false;
    }

    public void SetDestination(Transform target)
    {
        if(target)
        {
            walkPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            agent.isStopped = false;
        }
    }

    IEnumerator UpdateTarget()
    {
        if (refManager.exclusionTags.Count <= 0)
        {
            if (!targetFriendlies)
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString());
            else
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.friendlyTag.ToString());
        }
        else
        {
            if (!targetFriendlies)
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString(), refManager.exclusionTags);
            else
                chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.friendlyTag.ToString(), refManager.exclusionTags);
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
        if(agent && agent.isOnNavMesh)
            agent.destination = transform.position;

        refManager.aiAbility.DeactivateAbility();
        StopAgent();
    }

    public void StopAgent()
    {
        refManager.stateController.aiActive = false; 
        aiActive = false;
        if(agent && agent.isActiveAndEnabled)
            agent.isStopped = true;
    }

    public void StartAgent()
    {
        aiActive = true;
        refManager.stateController.currentState.CheckTransitions(refManager.stateController);
        refManager.stateController.aiActive = true;

        if(agent && agent.isActiveAndEnabled)
            agent.isStopped = false;
    }

    public void StartSelfDestruct(float effectTime)
    {
        if(!selfDestructing)
        {
            selfDestructing = true;
            StopAgent();
            StartCoroutine(SelfDestruct(effectTime));
        }
    }

    IEnumerator SelfDestruct(float effectTime)
    {
        if (selfDestructEffect)
            Instantiate(selfDestructEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(effectTime);
        refManager.targetManager.RemoveTarget(gameObject, refManager.friendlyTag.ToString());
        Destroy(gameObject);
    }
}
