using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsReloading : Conditional
{
    public SharedAbility ability;

    public override TaskStatus OnUpdate()
    {
        IReloadable reloadable = ability.Value as IReloadable;
        if (!reloadable.IsReloading())
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
