using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffects : MonoBehaviour
{
    public float baseSpeed;
    public List<Transform> waypoints;
    Health health;
    Vector3 targetPos;
    int currentWaypoint;
    
    Rigidbody rb;

    bool canAct;
    Coroutine stun;
    Coroutine slow;
    Coroutine cc;
    Coroutine root;
    Coroutine dot;
    Coroutine stackingDot;
    Coroutine stackingSlow;

    int dotStackAmount;
    int currentDotStacks;

    bool stackingDotActive;

    int currentSlowStacks;

    NavMeshAgent agent;

    private void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
        canAct = true;
    }

    #region Stun
    public void ApplyStun(float stunLength)
    {
        if (stun != null)
            StopCoroutine(stun);

        agent.speed = 0;
        canAct = false;
        print("Stunned " + agent.speed);

        stun = StartCoroutine(Stunned(stunLength));
    }

    IEnumerator Stunned(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        RemoveStun();
    }

    public void RemoveStun()
    {
        canAct = true;
        agent.speed = baseSpeed;
        print("Stun Removed " + agent.speed);
    }
#endregion
    #region Slow
    public void ApplySlow(float slowAmount, float slowLength)
    {
        if (slow != null)
            StopCoroutine(slow);

        print("Slow Amt " + slowAmount);
        float slowPercentage = slowAmount / 100;
        float newSpeed = (baseSpeed * slowPercentage);

        if(agent.speed != 0)
        {
            if (agent.speed - newSpeed >= 0)
                agent.speed -= newSpeed;
            else
                agent.speed = 0;
        }

        print("AI Slowed -- " + slowAmount + " Slow Percentage : " + slowPercentage + " New Speed : " + newSpeed + " Current Speed: " + agent.speed);

        slow = StartCoroutine(Slowed(slowLength));
    }

    IEnumerator Slowed(float slowLength)
    {
        yield return new WaitForSeconds(slowLength);
        RemoveSlow();
    }

    public void RemoveSlow()
    {
        agent.speed = baseSpeed;
        print("Slow Removed " + agent.speed);
    }
    #endregion
    #region KnockBack
    public void KnockedBack(float force)
    {
        rb.AddForce((-transform.forward + transform.up) * force, ForceMode.Force);
        print("Knocked Back");
    }
#endregion
    #region CC
    public void ApplyCC(float ccLength)
    {
        if (cc != null)
            StopCoroutine(cc);

        agent.speed = 0;
        canAct = false;
        cc = StartCoroutine(CC(ccLength));

        print("Enemy CC'd");
    }

    IEnumerator CC(float ccLength)
    {
        yield return new WaitForSeconds(ccLength);
        RemoveCC();
    }

    public void RemoveCC()
    {
        if (cc != null)
            StopCoroutine(cc);

        if(slow == null && root == null && stun == null)
        {
            agent.speed = baseSpeed;
            canAct = true;
        }

        print("CC Removed");
    }
#endregion
    #region Root
    public void ApplyRoot(float rootLength)
    {
        if (root != null)
            StopCoroutine(root);

        agent.speed = 0;
        root = StartCoroutine(Rooted(rootLength));

        print("Enemy rooted");
    }

    IEnumerator Rooted(float rootLength)
    {
        yield return new WaitForSeconds(rootLength);
        RemoveRoot();
    }

    public void RemoveRoot()
    {
        if (root != null)
            StopCoroutine(root);

        agent.speed = baseSpeed;

        print("Root Removed");
    }
    #endregion
    #region Stacking Dot

    public void StackDot(int dotDamage, float dotLength, int stackAmount)
    {
        if (stackingDot == null)
        {
            currentDotStacks++;
            dotStackAmount += dotDamage;
            stackingDot = StartCoroutine(StackingDot(dotLength));
        }
        else
        {
            StopCoroutine(stackingDot);
            if (currentDotStacks < stackAmount)
            {
                currentDotStacks++;
                dotStackAmount += dotDamage;
            }
            else
            {
                currentDotStacks = stackAmount;
                dotStackAmount = dotDamage * stackAmount;
            }
            stackingDot = StartCoroutine(StackingDot(dotLength));
        };
    }

    IEnumerator StackingDot(float dotLength)
    {
        for (int i = 0; i < dotLength; i++)
        {
            health.TookDamage(dotStackAmount);
            print("Dealing " + dotStackAmount + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
        currentDotStacks = 0;
        dotStackAmount = 0;
    }
    #endregion
    #region Stacking Slow

    public void StackSlow(float slowLength, int slowAmount, int stackAmount)
    {
        if (stackingSlow != null)
            StopCoroutine(stackingSlow);

        if (currentSlowStacks < stackAmount)
        {
            currentSlowStacks++;
        }
        else
        {
            currentSlowStacks = stackAmount;
        }

        stackingSlow = StartCoroutine(StackingSlow(slowAmount, slowLength));
    }

    IEnumerator StackingSlow(int slowAmount, float slowLength)
    {
        ApplySlow(slowAmount, slowLength);
        yield return new WaitForSeconds(slowLength);
        currentSlowStacks = 0;
    }
    #endregion
    #region SiphonHealth
    public void Siphon(int siphonDamage)
    {
        health.Heal(siphonDamage);
        print("Siphoned " + siphonDamage + " health from enemy");
    }
    #endregion
    #region DOT
    public void ApplyDOT(int dotDamage, float dotLength)
    {
        if (dot != null)
            StopCoroutine(dot);

        dot = StartCoroutine(DOT(dotDamage, dotLength));
    }

    IEnumerator DOT(int _dotDamage, float _dotLength)
    {
        for (int i = 0; i < _dotLength; i++)
        {
            health.TookDamage(_dotDamage);
            print("Dealing " + _dotDamage + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
}
