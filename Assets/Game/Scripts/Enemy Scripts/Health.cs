using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;

    int health;

    private void Start()
    {
        health = baseHealth;
    }

    public void TookDamage(int damage)
    {
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
