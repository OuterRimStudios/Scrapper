using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Random Walk")]
public class RandomWalkAction : AIAction
{
    public override void Act(StateController controller)
    {
        Move(controller);
    }

    void Move(StateController controller)
    {
   

        if (Utility.CheckDistance(controller.enemyRefManager.ai.walkPos, controller.enemyRefManager.ai.previousPos) <= .5f)
        {
            GetNewPos(controller);
        }

        controller.enemyRefManager.ai.agent.destination = controller.enemyRefManager.ai.walkPos;

        if(Utility.CheckDistance(controller.transform.position, controller.enemyRefManager.ai.walkPos) < .5f)
        {
            if (controller.enemyRefManager.animManager)
                controller.enemyRefManager.animManager.Idle();
        }
        else
        {
            if (controller.enemyRefManager.animManager)
                controller.enemyRefManager.animManager.Moving();
        }


        //walk//
        //controller.enemyRefManager.ai.StartAgent();
        // controller.enemyRefManager.ai.Move();
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
            Vector3 walkPoint = ray.GetPoint(Random.Range(4, controller.enemyRefManager.stats.moveRange));
            controller.enemyRefManager.ai.SetDestination(walkPoint);
        }
    }
}