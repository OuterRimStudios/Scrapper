﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Look")]
public class LookAtRootAction : Action {

    public float damping;
    public override void Act(StateController controller)
    {
        Look(controller);
    }

    void Look(StateController controller)
    {
        Vector3 targetPos = controller.enemyRefManager.ai.chaseTarget.root.position;
        Vector3 lookPos = new Vector3(targetPos.x, controller.transform.position.y, targetPos.z);

        var rotation = Quaternion.LookRotation(lookPos - controller.transform.position);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rotation, Time.deltaTime * damping);
    }
}
