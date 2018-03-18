using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffects : MonoBehaviour
{
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
    bool knockingBack;

    int currentSlowStacks;

    AI ai;

    private void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        ai = GetComponent<AI>();
        canAct = true;
    }

    #region Stun
    public void ApplyStun(float stunLength)
    {
        if (stun != null)
            StopCoroutine(stun);
        
        canAct = false;
        print("Stunned " + ai.speed);

        ai.InteruptAgent();
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
        ai.StartAgent();
        print("Stun Removed " + ai.speed);
    }
#endregion
    #region Slow
    public void ApplySlow(float slowAmount, float slowLength)
    {
        if (slow != null)
            StopCoroutine(slow);

        print("Slow Amt " + slowAmount);
        float slowPercentage = slowAmount / 100;
        float newSpeed = (ai.baseSpeed * slowPercentage);

        if(ai.speed != 0)
        {
            if (ai.speed - newSpeed >= 0)
                ai.speed -= newSpeed;
            else
                ai.speed = 0;
        }

        print("AI Slowed -- " + slowAmount + " Slow Percentage : " + slowPercentage + " New Speed : " + newSpeed + " Current Speed: " + ai.speed);

        slow = StartCoroutine(Slowed(slowLength));
    }

    IEnumerator Slowed(float slowLength)
    {
        yield return new WaitForSeconds(slowLength);
        RemoveSlow();
    }

    public void RemoveSlow()
    {
        ai.speed = ai.baseSpeed;
        print("Slow Removed " + ai.speed);
    }
    #endregion
    #region KnockBack
    public void KnockedBack(float force)
    {
        ai.InteruptAgent();
        rb.isKinematic = false;
        rb.AddForce((-transform.forward + transform.up) * force, ForceMode.Force);

        if (!knockingBack)
        {
            knockingBack = true;
            StartCoroutine(KnockedBack());
         }
        print("Knocked Back");
    }

    IEnumerator KnockedBack()
    {
        yield return new WaitForSeconds(1.5f);

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        ai.StartAgent();
        knockingBack = false;
    }
#endregion
    #region CC
    public void ApplyCC(float ccLength)
    {
        if (cc != null)
            StopCoroutine(cc);

        ai.InteruptAgent();
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
            ai.StartAgent();
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

        ai.speed = 0;
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

        ai.speed = ai.baseSpeed;

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
