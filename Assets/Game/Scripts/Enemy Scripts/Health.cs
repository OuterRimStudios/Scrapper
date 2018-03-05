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
    AI ai;

    private void Start()
    {
        health = baseHealth;
        ai = GetComponent<AI>();
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
        Destroy(gameObject);
    }
}
