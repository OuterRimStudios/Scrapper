using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public Sprite abilityIcon;
    [TextArea]
    public string abilityDescription;
    public float abilityCooldown;
    public bool requiresTarget;

    bool onCooldown;

    public virtual void ActivateAbility() { }
    public virtual void ReActivateAbility() { }

    public void TriggerCooldown()
    {
        onCooldown = true;
        StartCoroutine(Cooldown());
    }

    public bool OnCooldown()
    {
        return onCooldown;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }
}
