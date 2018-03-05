using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;

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

        if(health <= 0)
        {
            Died();
        }
    }

    void Died()
    {
        Destroy(gameObject);
    }
}
