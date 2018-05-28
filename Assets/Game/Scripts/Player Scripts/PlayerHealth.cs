using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public Image healthBar;
    public int lifeCount;

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDied;

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

    void Respawn()
    {
        transform.position = GameManager.instance.playerRespawnPoint.position;
        ResetHealth();
        isDead = false;
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)(health) / baseHealth;
    }

    protected override void Died()
    {
        OnPlayerDied();
        base.Died();
        if(lifeCount <= 0)
            LoadingScreenManager.LoadScene(0);
        else
            Respawn();
    }
}