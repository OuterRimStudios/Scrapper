using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Health : MonoBehaviour
{
    public int baseHealth;
    [HideInInspector] public bool isDead;

    protected int health;
    protected ReferenceManager refManager;

    protected virtual void Start()
    {
        refManager = GetComponent<ReferenceManager>();
        health = baseHealth;
    }

    protected virtual void OnEnable()
    {
        refManager = GetComponent<ReferenceManager>();
        isDead = false;
    }

    public float CheckHealthPercentage()
    {
        print("Current Health Percentage: " + (((float)health / baseHealth) * 100) + " << Current Health: " + health + " << Base Health: " + baseHealth);
        return (((float)health / baseHealth) * 100);
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

[System.Serializable]
public class SharedHealth : SharedVariable<Health>
{
    public static implicit operator SharedHealth(Health value) { return new SharedHealth { Value = value }; }
}