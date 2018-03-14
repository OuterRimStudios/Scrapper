using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public float damageMultiplier = 1;
    public int destructableLimbHealth;

    public bool limbActive;
    public bool destructable;
    Health health;

    float currentHealth;
    private void Start()
    {
        health = transform.root.GetComponent<Health>();

        currentHealth = destructableLimbHealth;

        if (damageMultiplier < 1)
            damageMultiplier = 1;
    }

    public void TookDamage(int damage)
    {
        print("Limb took damage " + Mathf.RoundToInt(damage * damageMultiplier));
        if (!limbActive) return;

        health.TookDamage(Mathf.RoundToInt(damage * damageMultiplier));

        if (destructable)
        {
            currentHealth -= Mathf.RoundToInt(damage * damageMultiplier);

            if (currentHealth <= 0)
                Destroy(gameObject);
        }

    }
}
