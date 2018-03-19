using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/SelfDestruct")]
public class SelfDestructAction : Action
{
    public float effectLength;

    public override void Act(StateController controller)
    {
        SelfDestruct(controller);
    }

    void SelfDestruct(StateController controller)
    {
        controller.enemyRefManager.ai.StartSelfDestruct(effectLength);
    }
}
