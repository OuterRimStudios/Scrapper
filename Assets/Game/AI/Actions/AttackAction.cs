using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AttackAction : Action
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
        if (Physics.SphereCast(controller.transform.position, controller.ai.stats.attackRadius, controller.transform.forward, out hit,
            controller.ai.stats.attackRange, attackLayer))
        {
            Debug.Log("Targeted");
            if (!controller.ai.attacking)
            {
                controller.ai.attacking = true;
                Debug.Log("Attacked");

                //hit.GetComponent<Health>().TookDamage(controller.ai.stats.damage);
                controller.StartCoroutine(controller.ai.Attack());
            }
        }
    }
}
