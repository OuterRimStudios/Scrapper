using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;
    [HideInInspector] public bool isDead;

    protected int health;
    protected ReferenceManager refManager;

    protected virtual void Start()
    {
        refManager = GetComponent<ReferenceManager>();
    }

    protected virtual void OnEnable()
    {
        refManager = GetComponent<ReferenceManager>();
        isDead = false;
    }

    public virtual void Heal(int healAmount)
    {
        if (health + healAmount <= baseHealth)
            health += healAmount;
        else
            health = baseHealth;
    }

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            Died();
        }
    }

    protected virtual void Died()
    {
        isDead = true;
    }
}