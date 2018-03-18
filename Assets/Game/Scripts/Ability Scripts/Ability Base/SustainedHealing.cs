using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SustainedHealing : Sustained
{
    public int healAmount;
    ReferenceManager refManager;

    protected override void Awake()
    {
        refManager = transform.root.GetComponent<ReferenceManager>();
        effectDelay = new WaitForSeconds(effectFrequency);
        layerMask = 1 << LayerMask.NameToLayer(friendlyTag);
    }

    public override void EffectOnTrigger(GameObject objectHit)
    {
        print(" >>>>>>>> Healing: " + objectHit.name + " <<<<<<<< ");

        if (objectHit.tag.Equals("Limb"))
            objectHit.transform.root.GetComponent<Health>().Heal(healAmount);
        else
            objectHit.GetComponent<Health>().Heal(healAmount);

        VisualOnTrigger();
        SpawnAfterEffects();
    }

    protected override IEnumerator RepeatEffect()
    {
        RaycastHit[] hitObjects = new RaycastHit[0];
        if (isTurret && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else if (shootForward && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, spawnPos.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else if (target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.forward * range, hitRadius, spawnPos.forward, range, layerMask);
        }

        if (hitObjects.Length > 0)
        {
            List<GameObject> hitObjectsList = new List<GameObject>();
            foreach (RaycastHit hit in hitObjects)
            {
                if (!hitObjectsList.Contains(hit.transform.gameObject))
                {
                    hitObjectsList.Add(hit.transform.gameObject);
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer(friendlyTag))
                    {
                        if(!refManager.exclusionTags.ToList().Contains(hit.transform.tag))
                        {
                            print("Healing : " + hit.transform.name + " Tag = " + hit.transform.tag);
                            EffectOnTrigger(hit.transform.gameObject);
                        }
                    }
                }
            }
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }
}
