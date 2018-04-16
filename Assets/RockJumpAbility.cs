using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockJumpAbility : Ability
{
    [Space, Header("Required Variables")]
    public PlayerMovement playerMovement;
    public float jumpForce;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        playerMovement.Jump(jumpForce);
        TriggerCooldown();
    }
}
