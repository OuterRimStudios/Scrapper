using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Look")]
public class LookDecision : Decision
{
    public LayerMask lookLayer;

    public override bool Decide(StateController controller)
    {
        bool targetVisable = Look(controller);                      //Return if the target is visable
        return targetVisable;
    }

    private bool Look(StateController controller)
    {
        RaycastHit hit;
        //Do we see a target
        if (Physics.SphereCast(controller.transform.position, controller.enemyRefManager.stats.lookRadius, controller.transform.forward, out hit,
            controller.enemyRefManager.stats.lookRange, lookLayer))
        {
            controller.enemyRefManager.ai.chaseTarget = hit.transform;             //Set the chaseTarget to the hit target
            return true;
        }
        else return false;
    }
}
