using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AbilityAction : Action
{
    public LayerMask attackLayer;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;
        //Are we close to the target
        if (Physics.SphereCast(controller.transform.position, controller.enemyRefManager.stats.attackRadius, controller.transform.forward, out hit,
            controller.enemyRefManager.stats.attackRange, attackLayer))
        {
            Debug.Log("Targeted");
            if (controller.enemyRefManager.aiAbility.CanShoot())
            {
                Debug.Log("Attacked");

                //hit.GetComponent<Health>().TookDamage(controller.ai.stats.damage);
                controller.enemyRefManager.aiAbility.ActivateAbility();
            }
        }
    }
}
