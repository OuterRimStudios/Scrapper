using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform[] patrolPoints;                                        //List of the way points to patrol through
    public Stats stats;                                                     //The Stats for this AI

    public Transform chaseTarget;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public bool attacking;

    WaitForSeconds attackFrequency;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackFrequency = new WaitForSeconds(1);
    }

    public IEnumerator Attack()
    {
        yield return attackFrequency;
        attacking = false;
    }
}
