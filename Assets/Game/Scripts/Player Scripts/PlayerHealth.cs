using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{

    protected override void Start()
    {
        base.Start();
        health = baseHealth;
    }

    public override void TookDamage(int damage)
    {
        base.TookDamage(damage);
    }
}