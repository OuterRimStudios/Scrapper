using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Charge")]
public class ChargeAction : Action
{
    public override void Act(StateController controller)
    {
        Charge(controller);
    }

    void Charge(StateController controller)
    {
        if (controller.enemyRefManager.ai.chaseTarget == null) return;
        controller.enemyRefManager.ai.agent.destination = controller.enemyRefManager.ai.walkPos;
        controller.enemyRefManager.ai.updateTarget = false;
    }
}