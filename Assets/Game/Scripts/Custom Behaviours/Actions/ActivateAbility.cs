using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ActivateAbility : Action
{
    public SharedAbility ability;

    public override TaskStatus OnUpdate()
    {
        ability.Value.ActivateAbility();
        return base.OnUpdate();
    }
}
