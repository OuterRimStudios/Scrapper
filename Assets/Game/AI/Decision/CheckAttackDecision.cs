using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Attack")]
public class CheckAttackDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (!controller.enemyRefManager.aiAbility.isCharging && !controller.enemyRefManager.aiAbility.isFiring)
            return false;   //no longer attacking
        else
            return true;
    }
}
