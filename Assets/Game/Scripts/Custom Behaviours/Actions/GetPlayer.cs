using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class GetPlayer : Action
{
    public SharedGameObject player;

    public override TaskStatus OnUpdate()
    {
        if (!player.Value)
            player.Value = TargetManager.instance.player;

        return TaskStatus.Success;
    }
}
