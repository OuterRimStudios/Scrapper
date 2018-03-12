using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        if (controller.enemyRefManager.ai.chaseTarget == null) return;

        controller.enemyRefManager.navMeshAgent.destination = controller.enemyRefManager.ai.chaseTarget.position;               //Start chasing
        controller.enemyRefManager.navMeshAgent.isStopped = false;
    }
}
