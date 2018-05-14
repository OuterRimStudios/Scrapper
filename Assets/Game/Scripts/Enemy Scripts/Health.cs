﻿using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;

    protected int health;
    protected ReferenceManager refManager;

    protected virtual void Start()
    {
        refManager = GetComponent<ReferenceManager>();
    }

    protected virtual void OnEnable()
    {
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
        health -= damage;

        if (health <= 0)
        {
            Died();
        }
    }

    protected virtual void Died()
    {
        if(refManager.targetManager)
        refManager.targetManager.RemoveTarget(gameObject, refManager.friendlyTag.ToString());
        Destroy(gameObject);
    }
}