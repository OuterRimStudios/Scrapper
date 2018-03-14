using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIHealth : Health
{
    public GameObject combatText;
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
        cbt.GetComponent<Text>().text = damage.ToString();
    }

    protected override void Died()
    {
        base.Died();
    }
}