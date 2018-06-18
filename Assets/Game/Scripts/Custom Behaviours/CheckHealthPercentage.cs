using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CheckHealthPercentage : Conditional
{
    public float desiredHealthPercentage;
    public AIHealth health;

    public override TaskStatus OnUpdate()
    {
        if(health.CheckHealthPercentage() <= desiredHealthPercentage)
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
