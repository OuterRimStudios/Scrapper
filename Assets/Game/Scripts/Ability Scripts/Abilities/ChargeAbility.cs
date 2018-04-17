using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAbility : Ability
{
    [Space, Header("Required Variables")]
    public PlayerMovement playerMovement;
    public int chargeDistance;
    public float chargeSpeed;
    public LayerMask environmentMask;

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        RaycastHit hit;
        Ray ray = new Ray(refManager.transform.position, refManager.transform.forward);
        if(Physics.Raycast(ray, out hit, chargeDistance, environmentMask))
            playerMovement.Charge(hit.point - (hit.normal * 2), chargeSpeed);
        else
            playerMovement.Charge(ray.GetPoint(chargeDistance), chargeSpeed);

        TriggerCooldown();
    }
}
