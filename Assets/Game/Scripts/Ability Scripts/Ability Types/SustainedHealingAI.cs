using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedHealingAI : Ability
{
    public SustainedHealing beamPrefab;
    public float effectLength;
    public int initialDamage;

    [HideInInspector] public List<SustainedHealing> beams = new List<SustainedHealing>();

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < refManager.SpawnPosition().Length; i++)
        {
            beams.Add(Instantiate(beamPrefab));
            UpdateTransform();
            beams[i].Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects, refManager.SpawnPosition()[i]);
            beams[i].gameObject.SetActive(false);
        }
    }

    public override void VisualOnActivate()
    {
        AIHealth health = (AIHealth)refManager.health;
        health.SetLimbsActive(true);
        refManager.animManager.Attack();
    }

    public override void VisualOnDeactivate()
    {
        AIHealth health = (AIHealth)refManager.health;
        health.SetLimbsActive(false);
        refManager.animManager.StopAttack();
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }

        isCharging = false;         //Interupts Attack phase if using attack decision
        isFiring = false;

        VisualOnDeactivate();
        //UpdateTransform();
        TriggerCooldown();
    }

    public override void ModuleActivated(ModuleAbility module)
    {
    
    }

    IEnumerator Firing()
    {
        isFiring = true;
        VisualOnActivate();
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].SetTarget(refManager.targetManager.GetClosestTarget(transform.position, refManager.friendlyTag.ToString()), true);
            beams[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(effectLength);
        DeactivateAbility();
        isFiring = false;
    }

    protected override IEnumerator Charge()
    {
        isCharging = true;
        if (!CheckIfFriendly())
        {
            if (refManager.GetType() == typeof(AIReferenceManager))
            {
                AIReferenceManager tempManager = (AIReferenceManager)refManager;
                tempManager.ai.StopAgent();
            }
        }
        yield return new WaitForSeconds(chargeTime);
        if (!CheckIfFriendly())
        {
            if (refManager.GetType() == typeof(AIReferenceManager))
            {
                AIReferenceManager tempManager = (AIReferenceManager)refManager;
                tempManager.ai.StartAgent();
            }
        }
        isCharging = false;

        if (!isFiring)
            StartCoroutine(Firing());
    }

    void UpdateTransform()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].transform.SetParent(refManager.SpawnPosition()[i]);
            beams[i].transform.position = refManager.SpawnPosition()[i].position;
            beams[i].transform.rotation = refManager.SpawnPosition()[i].rotation;
        }
    }
}
