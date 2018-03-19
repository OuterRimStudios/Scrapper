using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/CheckForEnemyTargets")]
public class CheckForEnemyTargetsDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        float length = 0;
        if (controller.enemyRefManager.friendlyTag == ReferenceManager.Tag.Friendly)
        {
            length = Utility.CullList(controller.enemyRefManager.targetManager.activeEnemies, controller.enemyRefManager.exclusionTags).Count;
        }
        else if(controller.enemyRefManager.friendlyTag == ReferenceManager.Tag.Enemy)
        {
            length = Utility.CullList(controller.enemyRefManager.targetManager.activeFriendlies, controller.enemyRefManager.exclusionTags).Count;
        }
        if (length > 0)
            return true;
        else return false;
    }
}
