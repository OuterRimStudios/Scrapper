using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CheckCurrentPhase : Conditional
{
    public float desiredPhase;
    public SharedPhaseManager phaseManager;

    public override TaskStatus OnUpdate()
    {
        if (phaseManager.Value.GetCurrentPhase() == desiredPhase)
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
