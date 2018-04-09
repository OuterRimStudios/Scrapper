using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public Image healthBar;

    protected override void Start()
    {
        base.Start();
        ResetHealth();
    }

    public override void TookDamage(int damage)
    {
        base.TookDamage(damage);
        UpdateHealthBar();
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        UpdateHealthBar();
    }

    void ResetHealth()
    {
        health = baseHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)(health) / baseHealth;
    }
}