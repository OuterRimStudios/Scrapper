using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SetNewPhase : Action
{
    public SharedPhaseManager phaseManager;
    public int newPhase;

    public override void OnStart()
    {
        phaseManager.Value.SetPhase(newPhase);
        base.OnStart();
    }
}

