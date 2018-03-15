using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;

    protected int health;
    protected ReferenceManager refManager;

    protected virtual void Start()
    {
        health = baseHealth;
        refManager = GetComponent<ReferenceManager>();
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
        print(gameObject.name + " took damage: " + damage);
        health -= damage;

        if (health <= 0)
        {
            Died();
        }
    }

    protected virtual void Died()
    {
        refManager.targetManager.RemoveTarget(gameObject, refManager.friendlyTag.ToString());
        Destroy(gameObject);
    }
}