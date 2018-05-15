using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle")]
public class IdleAction : StateMachineAction
{
    public override void Act(StateController controller)
    {
        Idle(controller);
    }

    private void Idle(StateController controller)
    {
        if (!controller.enemyRefManager.animManager) return;

        controller.enemyRefManager.animManager.Idle();
    }
}
