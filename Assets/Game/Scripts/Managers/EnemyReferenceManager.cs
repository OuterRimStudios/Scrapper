using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReferenceManager : ReferenceManager {

    [HideInInspector] public AI ai;
    [HideInInspector] public StateController stateController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public Ability aiAbility;

    protected override void Awake()
    {
        base.Awake();

        ai = GetComponent<AI>();
        stateController = GetComponent<StateController>();
        statusEffects = GetComponent<StatusEffects>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
}