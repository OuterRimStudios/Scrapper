using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AbilityOnCooldown : Conditional
{
    public Ability ability;

    public override TaskStatus OnUpdate()
    {
        if (!ability.CanShoot())
            return TaskStatus.Failure;
        else return TaskStatus.Success;
    }
}
