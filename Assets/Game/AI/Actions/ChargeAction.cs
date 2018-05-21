using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Charge")]
public class ChargeAction : AIAction
{
    public override void Act(StateController controller)
    {
        Charge(controller);
    }

    void Charge(StateController controller)
    {
        Debug.LogError("No Charge Logic");
    }
}