using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ActivateAbility : Action
{
    public Ability ability;

    public override TaskStatus OnUpdate()
    {
        ability.ActivateAbility();
        return base.OnUpdate();
    }
}
