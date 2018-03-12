using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public GameObject combatText;
    public GameObject enemyCanvas;

    StatusEffects ai;
    Coroutine dot;

    protected override void Start()
    {
        base.Start();
        ai = GetComponent<StatusEffects>();
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