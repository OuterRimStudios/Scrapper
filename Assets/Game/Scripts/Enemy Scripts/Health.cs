using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int baseHealth;

    public GameObject combatText;
    public GameObject enemyCanvas;

    int health;
    StatusEffects ai;
    Coroutine dot;

    TargetManager targetManager;

    private void Start()
    {
        health = baseHealth;
        ai = GetComponent<StatusEffects>();
        targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
    }

    public void TookDamage(int damage)
    {
        ai.RemoveCC();
        health -= damage;

        GameObject cbt = Instantiate(combatText, enemyCanvas.transform.position, enemyCanvas.transform.rotation, enemyCanvas.transform);
        cbt.GetComponent<Text>().text = damage.ToString();

        if (health <= 0)
        {
            Died();
        }
    }

    void Died()
    {
        targetManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    #region DOT
    public void ApplyDOT(int dotDamage, float dotLength)
    {
        if (dot != null)
            StopCoroutine(dot);

        dot = StartCoroutine(DOT(dotDamage, dotLength));
    }

    IEnumerator DOT(int _dotDamage, float _dotLength)
    {
        for (int i = 0; i < _dotLength; i++)
        {
            TookDamage(_dotDamage);
            print("Dealing " + _dotDamage + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
}