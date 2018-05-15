using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Chase")]
public class ChaseAction : StateMachineAction
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        if (controller.enemyRefManager.ai.chaseTarget == null) return;

        controller.enemyRefManager.ai.agent.destination = controller.enemyRefManager.ai.walkPos;
      //  controller.enemyRefManager.ai.Move();
    }
}
