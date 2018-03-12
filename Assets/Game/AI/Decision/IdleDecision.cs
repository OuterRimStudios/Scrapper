using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Idle")]
public class IdleDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return controller.isIdle;
    }
}
