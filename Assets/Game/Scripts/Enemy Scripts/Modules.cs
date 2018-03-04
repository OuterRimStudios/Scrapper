﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modules : MonoBehaviour
{
    bool concussionActive;
    bool incineratingActive;
    bool weightedActive;
    bool siphonActive;
    bool hemorrhageActive;

    Coroutine crippled;
    int currentCrippledStacks;

    Coroutine hemorrhaging;
    int hemorrhagingAmount;
    int currentHemorrhagingStacks;

    GameObject player;
    PlayerHealth playerHealth;
    Health health;
    AI ai;

    private void Start()
    {
        ai = GetComponent<AI>();
        health = GetComponent<Health>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void Concussion(float concussionLength)
    {
        if(!concussionActive)
        {
            concussionActive = true;
            StartCoroutine(Concussed(concussionLength));
        }
    }

    IEnumerator Concussed(float concussionLength)
    {
        ai.ApplyStun();
        yield return new WaitForSeconds(concussionLength);
        ai.RemoveStun();
        concussionActive = false;
    }

    public void Incinerating(int incineratingDamage, float incineratingLength)
    {
        if(!incineratingActive)
        {
            incineratingActive = true;
            StartCoroutine(Incinerated(incineratingDamage, incineratingLength));
        }
    }

    IEnumerator Incinerated(int incineratingDamage, float incineratingLength)
    {
        for(int i = 0; i <incineratingLength; i++)
        {
            health.TookDamage(incineratingDamage);
            print("Dealing " + incineratingDamage + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
        incineratingActive = false;
    }

    public void Crippling(float cripplingLength, int slowAmount, int stackAmount)
    {
        if(crippled == null)
        {
            currentCrippledStacks++;
            
            crippled = StartCoroutine(Crippled(slowAmount, cripplingLength));
        }
        else
        {
            StopCoroutine(crippled);
            if(currentCrippledStacks < stackAmount)
            {
                currentCrippledStacks++;
            }
            else
            {
                currentCrippledStacks = stackAmount;
            }
            crippled = StartCoroutine(Crippled(slowAmount, cripplingLength));
        }
    }

    IEnumerator Crippled(int slowAmount, float cripplingLength)
    {
        ai.ApplySlow(slowAmount);
        yield return new WaitForSeconds(cripplingLength);
        currentCrippledStacks = 0;
        ai.RemoveSlow();
    }

    public void Weighted(float weightedForce)
    {
        ai.KnockedBack(weightedForce);
    }
    public void Siphon(int siphonDamage)
    {
        playerHealth.Heal(siphonDamage);
        print("Siphoned " + siphonDamage + " health from enemy");
    }

    public void Hemorrhage(int hemorrhageDamage, float hemorrhageLength, int stackAmount)
    {
        if (hemorrhaging == null)
        {
            currentHemorrhagingStacks++;
            hemorrhagingAmount += hemorrhageDamage;
            hemorrhaging = StartCoroutine(Hemorrhaging(hemorrhageLength));
        }
        else
        {
            StopCoroutine(hemorrhaging);
            if (currentHemorrhagingStacks < stackAmount)
            {
                currentHemorrhagingStacks++;
                hemorrhagingAmount += hemorrhageDamage;
            }
            else
            {
                currentHemorrhagingStacks = stackAmount;
                hemorrhagingAmount = hemorrhageDamage * stackAmount;
            }
            hemorrhaging = StartCoroutine(Hemorrhaging(hemorrhageLength));
        };
    }

    IEnumerator Hemorrhaging(float hemorrhageLength)
    {
        for (int i = 0; i < hemorrhageLength; i++)
        {
            health.TookDamage(hemorrhagingAmount);
            print("Dealing " + hemorrhagingAmount + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
        currentHemorrhagingStacks = 0;
        hemorrhagingAmount = 0;
    }
}
