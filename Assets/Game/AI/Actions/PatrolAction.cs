using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : Action
{               
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.ai.agent.destination = controller.ai.patrolPoints[controller.ai.nextWayPoint].position;                      //Set the agent's next waypoint
        controller.ai.agent.isStopped = false;
        
        if(controller.ai.agent.remainingDistance <= controller.ai.agent.stoppingDistance && !controller.ai.agent.pathPending)   //Are we at our destination?
        {
            controller.ai.nextWayPoint = (controller.ai.nextWayPoint + 1) % controller.ai.patrolPoints.Length;                  //Increament nextWayPoint or reset it to 0
        }
    }
}
