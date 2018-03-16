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

    protected override void Start()
    {
        base.Start();
        ai = GetComponent<StatusEffects>();

        foreach(Limb limb in transform.GetComponentsInChildren<Limb>())
        {
            limbs.Add(limb);
        }
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
        AIReferenceManager tempManager = (AIReferenceManager)refManager;
        tempManager.targetManager.RemoveTarget(gameObject, refManager.friendlyTag.ToString(), tempManager.ai.chaseTarget);
        base.Died();
    }
}