using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIHealth : Health
{
    public GameObject combatText;
    public GameObject healingText;
    public GameObject enemyCanvas;

    StatusEffects ai;
    Coroutine dot;

    List<Limb> limbs = new List<Limb>();
    AIReferenceManager aiRefManager;
    EncounterSpawner encounterSpawner;

    protected override void Start()
    {
        base.Start();
        ai = GetComponent<StatusEffects>();

        aiRefManager = refManager as AIReferenceManager;

        if(aiRefManager.currentChallengeTier)
        health = aiRefManager.currentChallengeTier.health;

        health = baseHealth;

        foreach (Limb limb in transform.GetComponentsInChildren<Limb>())
        {
            limbs.Add(limb);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ai = GetComponent<StatusEffects>();

        aiRefManager = refManager as AIReferenceManager;
        if(aiRefManager.currentChallengeTier)
        health = aiRefManager.currentChallengeTier.health;

        foreach (Limb limb in transform.GetComponentsInChildren<Limb>())
        {
            limbs.Add(limb);
        }
    }

    public void SetEncounterSpawner(EncounterSpawner _encounterSpawner)
    {
        encounterSpawner = _encounterSpawner;
    }

    public void SetLimbsActive(bool activeState)
    {
        if (limbs.Count <= 0) return;
        for (int i = 0; i < limbs.Count; i++)
            limbs[i].limbActive = activeState;
    }

    public override void TookDamage(int damage)
    {
        base.TookDamage(damage);
        ai.RemoveCC();

        GameObject cbt = Instantiate(combatText, enemyCanvas.transform.position, enemyCanvas.transform.rotation, enemyCanvas.transform);
        Text cbtText = cbt.GetComponent<Text>();
        cbtText.text = damage.ToString();
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);

        GameObject cbt = Instantiate(healingText, enemyCanvas.transform.position, enemyCanvas.transform.rotation, enemyCanvas.transform);
        Text cbtText = cbt.GetComponent<Text>();
        cbtText.text = healAmount.ToString();
    }

    protected override void Died()
    {
        if (encounterSpawner)
            encounterSpawner.RemoveEnemy(gameObject);

        base.Died();
        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        if(refManager.targetManager)
            refManager.targetManager.RemoveTarget(gameObject, refManager.friendlyTag.ToString());
        Destroy(gameObject);
    }
}