using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public AIReferenceManager refManager;
    public GameObject selfDestructEffect;

    public float speed;
    WaitForSeconds waitTime;
    bool aiActive; 
    bool selfDestructing;

    Rigidbody rb;
    [HideInInspector] public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(agent)
        speed = agent.speed;
    }
    public void InteruptAgent()
    {
        if(agent && agent.isOnNavMesh)
            agent.destination = transform.position;

        foreach(Ability ability in refManager.aiAbilities)
            ability.DeactivateAbility();

        StopAgent();
    }

    public void StopAgent()
    {
      //  refManager.stateController.aiActive = false; 
        aiActive = false;
        if(agent && agent.isActiveAndEnabled)
            agent.isStopped = true;
    }

    public void StartAgent()
    {
        aiActive = true;
      //  refManager.stateController.currentState.CheckTransitions(refManager.stateController);
       // refManager.stateController.aiActive = true;

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
