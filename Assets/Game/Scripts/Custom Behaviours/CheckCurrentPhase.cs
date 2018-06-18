using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CheckCurrentPhase : Conditional
{
    public float desiredPhase;
    public PhaseManager phaseManager;

    public override TaskStatus OnUpdate()
    {
        if (phaseManager.GetCurrentPhase() == desiredPhase)
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
