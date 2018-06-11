using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Attack")]
public class CheckAttackDecision : Decision
{
    public string abilityName;

    Ability ability;

    public override bool Decide(StateController controller)
    {
        if (!ability)
            ability = controller.enemyRefManager.GetAbility(abilityName);

        if (!ability.isCharging && !ability.isFiring)
            return false;   //no longer attacking
        else
            return true;
    }
}
