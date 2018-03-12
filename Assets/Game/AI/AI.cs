using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public ReferenceManager refManager;
    public Transform[] patrolPoints;                                        //List of the way points to patrol through

    public Transform chaseTarget;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public bool attacking;

    WaitForSeconds attackFrequency;

    public float updateTargetFrequency;                                      //How often the AI will update it's state
    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingState;                                                     //Checks if it's time to update the state
    bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes

    private void Start()
    {
        attackFrequency = new WaitForSeconds(1);
        waitTime = new WaitForSeconds(updateTargetFrequency);
    }

    void Update()
    {
        if (!aiActive) return;

        if (!updatingState)
        {
            updatingState = true;
            StartCoroutine(UpdateTarget());
        }
    }

    IEnumerator UpdateTarget()
    {
        chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString());
        yield return waitTime;
        updatingState = false;
    }

    public IEnumerator Attack()
    {
        yield return attackFrequency;
        attacking = false;
    }
}
