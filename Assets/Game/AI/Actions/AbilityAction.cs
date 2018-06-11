using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AbilityAction : AIAction
{
    public string abilityName;

    Ability ability;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        if (!ability)
            ability = controller.enemyRefManager.GetAbility(abilityName);

        if (ability.CanShoot())
        {
            ability.ActivateAbility();
        }
    }
}
