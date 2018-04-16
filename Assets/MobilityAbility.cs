using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityAbility : Ability
{
    public PlayerMovement playerMovement;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        playerMovement.Sprint(true);
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        playerMovement.Sprint(false);
        TriggerCooldown();
    }
}
