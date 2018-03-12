using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.enemyRefManager.ai.chaseTarget == null) return false;
        bool targetIsActive = controller.enemyRefManager.ai.chaseTarget.gameObject.activeSelf;
        return targetIsActive;
    }
}
