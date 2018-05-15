using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : StateMachineAction
{
    Patrol patrol;
    public override void Act(StateController controller)
    {
        if(!patrol)
        patrol = GameObject.Find("GameManager").GetComponent<Patrol>();
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.enemyRefManager.ai.agent.destination = patrol.patrolPoints[controller.enemyRefManager.ai.nextWayPoint].position;                      //Set the agent's next waypoint
        controller.enemyRefManager.ai.agent.isStopped = false;

        if (controller.enemyRefManager.ai.agent.remainingDistance <= controller.enemyRefManager.ai.agent.stoppingDistance && !controller.enemyRefManager.ai.agent.pathPending)   //Are we at our destination?
        {
            controller.enemyRefManager.ai.nextWayPoint = (controller.enemyRefManager.ai.nextWayPoint + 1) % patrol.patrolPoints.Length;                  //Increament nextWayPoint or reset it to 0
        }
    }
}
