using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Look")]
public class LookAction : Action
{
    public override void Act(StateController controller)
    {
        Look(controller);
    }

    void Look(StateController controller)
    {
        Vector3 targetPos = controller.enemyRefManager.ai.walkPos;
        Vector3 lookPos = new Vector3(targetPos.x, controller.transform.position.y, targetPos.z);
        controller.transform.LookAt(lookPos);
    }
}