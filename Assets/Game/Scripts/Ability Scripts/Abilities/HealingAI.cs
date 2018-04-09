using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAI : FriendlyAI
{
    public float effectLength;
    public float effectCooldown;

    bool healing;
    List<Sustained> beams = new List<Sustained>();

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            beams.Add(Instantiate(aiAbility) as Sustained);
            beams[i].Initialize(damage, enemyTag, friendlyTag, afterEffects, spawnpoints[i]);
            UpdateTransform();
        }
    }

    public override void Update()
    {
        base.Update();
        if (!target) return;
        if (!healing)
        {
            healing = true;
            StartCoroutine(Healing());
        }
    }

    void UpdateTransform()
    {
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            beams[i].transform.SetParent(spawnpoints[i]);
            beams[i].transform.position = spawnpoints[i].position;
            beams[i].transform.rotation = spawnpoints[i].rotation;
        }
    }

    public override void ModuleActivated(ModuleAbility module)
    {

    }

    protected override void TargetUpdated()
    {
        base.TargetUpdated();
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].SetTarget(target, true);
        }
    }

    IEnumerator Healing()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(effectLength);

        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(effectCooldown);

        healing = false;
    }
}
