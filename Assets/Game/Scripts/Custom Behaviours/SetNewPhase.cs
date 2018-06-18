using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SetNewPhase : Action
{
    public PhaseManager phaseManager;
    public int newPhase;

    public override void OnStart()
    {
        phaseManager.SetPhase(newPhase);
        base.OnStart();
    }
}

