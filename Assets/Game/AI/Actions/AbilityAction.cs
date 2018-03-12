using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AbilityAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        if (controller.enemyRefManager.aiAbility.CanShoot())
        {
            controller.enemyRefManager.aiAbility.ActivateAbility();
        }
    }
}
