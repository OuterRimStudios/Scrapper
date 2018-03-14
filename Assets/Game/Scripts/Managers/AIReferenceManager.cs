using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIReferenceManager : ReferenceManager {

    [HideInInspector] public AI ai;
    [HideInInspector] public StateController stateController;
    public Ability aiAbility;

    protected override void Awake()
    {
        base.Awake();

        ai = GetComponent<AI>();
        stateController = GetComponent<StateController>();
        statusEffects = GetComponent<StatusEffects>();
    }
}