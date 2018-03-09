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
        controller.ai.agent.destination = controller.ai.chaseTarget.position;               //Start chasing
        controller.ai.agent.isStopped = false;
    }
}
