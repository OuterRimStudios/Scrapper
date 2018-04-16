using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Position")]
public class CheckPositionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (Utility.CheckDistance(controller.transform.position, controller.enemyRefManager.ai.walkPos) <= controller.enemyRefManager.stats.personalSpaceRange)   //Are we at our destination?
        {
            if (!controller.enemyRefManager.ai.agent || !controller.enemyRefManager.ai.agent.isOnNavMesh)
                return false;
            controller.enemyRefManager.ai.agent.isStopped = true;
            controller.enemyRefManager.ai.previousPos = controller.transform.position;
            return true;
        }
        else
            return false;
    }
}
