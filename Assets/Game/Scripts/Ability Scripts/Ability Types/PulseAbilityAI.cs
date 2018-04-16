using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseAbilityAI : Ability
{
    public GameObject pulseEffect;
    public float pulseRadius;
    public int initialDamage;

    bool dealDamage;

    public override void VisualOnActivate()
    {
        pulseEffect.SetActive(true);
        //refManager.animManager.Attack();
    }

    public override void VisualOnDeactivate()
    {
        pulseEffect.SetActive(false);
        //refManager.animManager.StopAttack();
    }

    public override void ActivateAbility()
    {
        VisualOnActivate();
        dealDamage = true;

        RaycastHit[] hitObjects = Physics.SphereCastAll(transform.position, pulseRadius, transform.position, 0);

        foreach (RaycastHit hit in hitObjects)
        {
            if (hit.transform.tag.Equals("Friendly") && dealDamage)
            {
                dealDamage = false;
                hit.transform.GetComponent<Health>().TookDamage(initialDamage);
            }
        }

        base.ActivateAbility();
    }

    protected override IEnumerator Charge()
    {
        isCharging = true;
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
        DeactivateAbility();
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();

        VisualOnDeactivate();
        TriggerCooldown();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pulseRadius);
        Gizmos.color = Color.blue;
    }
}
