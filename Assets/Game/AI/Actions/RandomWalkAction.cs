using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Random Walk")]
public class RandomWalkAction : Action
{
    public override void Act(StateController controller)
    {
        Move(controller);
    }

    void Move(StateController controller)
    {
        if (controller.enemyRefManager.ai.walkPos == controller.enemyRefManager.ai.previousPos)
        {
            GetNewPos(controller);
        }

        //walk
        controller.enemyRefManager.navMeshAgent.destination = controller.enemyRefManager.ai.walkPos;
        controller.enemyRefManager.navMeshAgent.isStopped = false;
    }

    void GetNewPos(StateController controller)
    {
        Ray ray = new Ray(controller.transform.position, new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, controller.enemyRefManager.stats.moveRange, controller.enemyRefManager.boundLayer))
        {
            GetNewPos(controller);
        }
        else
        {
            controller.enemyRefManager.ai.walkPos = ray.GetPoint(Random.Range(4, controller.enemyRefManager.stats.moveRange));
        }
    }
}