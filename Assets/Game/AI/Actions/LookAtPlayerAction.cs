using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Look At Player")]
public class LookAtPlayerAction : Action {
    public float damping;

    public override void Act(StateController controller)
    {
        Look(controller);
    }

    void Look(StateController controller)
    {
        if (controller.enemyRefManager.targetManager.player == null) return;

        Vector3 targetPos = controller.enemyRefManager.targetManager.player.transform.position;
        Vector3 lookPos = new Vector3(targetPos.x, controller.transform.position.y, targetPos.z);
        var rotation = Quaternion.LookRotation(lookPos - controller.transform.position);        
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * damping);
    }
}
