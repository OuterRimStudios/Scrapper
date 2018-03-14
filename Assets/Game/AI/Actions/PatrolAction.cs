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
        //controller.enemyRefManager.navMeshAgent.destination = controller.enemyRefManager.ai.patrolPoints[controller.enemyRefManager.ai.nextWayPoint].position;                      //Set the agent's next waypoint
        //controller.enemyRefManager.navMeshAgent.isStopped = false;
        
        //if(controller.enemyRefManager.navMeshAgent.remainingDistance <= controller.enemyRefManager.navMeshAgent.stoppingDistance && !controller.enemyRefManager.navMeshAgent.pathPending)   //Are we at our destination?
        //{
        //    controller.enemyRefManager.ai.nextWayPoint = (controller.enemyRefManager.ai.nextWayPoint + 1) % controller.enemyRefManager.ai.patrolPoints.Length;                  //Increament nextWayPoint or reset it to 0
        //}
    }
}
