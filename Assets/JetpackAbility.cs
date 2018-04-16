using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackAbility : Ability
{
    [Space, Header("Required Variables")]
    public PlayerMovement playerMovement;
    public float hoverLength;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        playerMovement.Hover(true);
        StartCoroutine(HoverLength());
        TriggerCooldown();
    }

    IEnumerator HoverLength()
    {
        yield return new WaitForSeconds(hoverLength);
        playerMovement.Hover(false);
    }
}
