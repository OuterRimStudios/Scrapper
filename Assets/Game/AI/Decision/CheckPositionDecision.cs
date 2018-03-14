using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Position")]
public class CheckPositionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (Utility.CheckDistance(controller.transform.position, controller.enemyRefManager.ai.walkPos) <= .5f)   //Are we at our destination?
        {
            controller.enemyRefManager.ai.previousPos = controller.transform.position;
            return true;
        }
        else
            return false;
    }
}
