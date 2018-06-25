using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AbilityOnCooldown : Conditional
{
    public SharedAbility ability;

    public override TaskStatus OnUpdate()
    {
        if (!ability.Value.CanShoot())
            return TaskStatus.Failure;
        else return TaskStatus.Success;
    }
}
