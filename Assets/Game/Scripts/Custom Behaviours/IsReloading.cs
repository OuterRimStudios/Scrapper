using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsReloading : Conditional
{
    public Ability ability;

    public override TaskStatus OnUpdate()
    {
        IReloadable reloadable = ability as IReloadable;
        if (!reloadable.IsReloading())
            return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
