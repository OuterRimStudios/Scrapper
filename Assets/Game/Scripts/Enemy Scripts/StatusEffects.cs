using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffects : MonoBehaviour
{
    Health health;
    Vector3 targetPos;
    int currentWaypoint;
    
    Rigidbody rb;

    [Header("Status Effects")]
    public GameObject stunEffect;
    public GameObject slowEffect;
    public GameObject ccEffect;
    public GameObject dotEffect;
    public GameObject stackingDotEffect;
    public GameObject stackingSlowEffect;
    public GameObject siphonEffect;

    [Space, Header("Ability Effects")]
    public GameObject containmentZone;

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
    Health playerHealth;

    private void Start()
    {
        health = GetComponent<Health>();
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        ai = GetComponent<AI>();
        canAct = true;
    }

    #region Stun
    public void ApplyStun(float stunLength)
    {
        if (stun != null)
            StopCoroutine(stun);

        if (!ai) return;
        
        canAct = false;

        ai.InteruptAgent();
        stunEffect.SetActive(true);
        stun = StartCoroutine(Stunned(stunLength));
    }

    IEnumerator Stunned(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        RemoveStun();
    }

    public void RemoveStun()
    {
        stunEffect.SetActive(false);
        canAct = true;
        ai.StartAgent();
    }
#endregion
    #region Slow
    public void ApplySlow(float slowAmount, float slowLength)
    {
        if (slow != null)
            StopCoroutine(slow);

        if (!ai) return;

        float slowPercentage = slowAmount / 100;
        float newSpeed = (ai.speed * slowPercentage);

        if(ai.agent.speed != 0)
        {
            if (ai.agent.speed - newSpeed >= 0)
                ai.agent.speed -= newSpeed;
            else
                ai.agent.speed = 0;
        }
     
        slowEffect.SetActive(true);
        slow = StartCoroutine(Slowed(slowLength));
    }

    IEnumerator Slowed(float slowLength)
    {
        yield return new WaitForSeconds(slowLength);
        RemoveSlow();
    }

    public void RemoveSlow()
    {
        slowEffect.SetActive(false);
        if(ai.agent)
        ai.agent.speed = ai.speed;
    }
    #endregion
    #region KnockBack
    public void KnockedBack(float force)
    {
        if (!ai) return;
        ai.InteruptAgent();
        rb.isKinematic = false;
        rb.AddForce((-transform.forward + transform.up) * force, ForceMode.Force);

        if (!knockingBack)
        {
            knockingBack = true;
            StartCoroutine(KnockedBack());
         }
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
        if (!ai) return;

        ai.InteruptAgent();
        canAct = false;

        ccEffect.SetActive(true);
        cc = StartCoroutine(CC(ccLength));
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
            ccEffect.SetActive(false);
            ai.StartAgent();
            canAct = true;
        }
    }
#endregion
    #region Root
    public void ApplyRoot(float rootLength)
    {
        if (root != null)
            StopCoroutine(root);
        if (!ai) return;
        ai.agent.speed = 0;
        
        root = StartCoroutine(Rooted(rootLength));
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
        if (ai.agent)
            ai.agent.speed = ai.speed;
    }
    #endregion
    #region Stacking Dot

    public void StackDot(int dotDamage, float dotLength, int stackAmount)
    {
        if (!ai) return;
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
            stackingDotEffect.SetActive(true);
            health.TookDamage(dotStackAmount);
            yield return new WaitForSeconds(1);

            stackingDotEffect.SetActive(false);
        }
        currentDotStacks = 0;
        dotStackAmount = 0;
    }
    #endregion
    #region Stacking Slow

    public void StackSlow(float slowLength, int slowAmount, int stackAmount)
    {
        if (!ai) return;
        if (stackingSlow != null)
            StopCoroutine(stackingSlow);

        stackingSlowEffect.SetActive(true);

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

        stackingSlowEffect.SetActive(false);
        currentSlowStacks = 0;
    }
    #endregion
    #region SiphonHealth
    public void Siphon(int siphonDamage)
    {
        if (!ai) return;
        siphonEffect.SetActive(true);
        playerHealth.Heal(siphonDamage);
    }
    #endregion
    #region DOT
    public void ApplyDOT(int dotDamage, float dotLength)
    {
        if (!ai) return;
        if (dot != null)
            StopCoroutine(dot);

        dotEffect.SetActive(true);
        dot = StartCoroutine(DOT(dotDamage, dotLength));
    }

    IEnumerator DOT(int _dotDamage, float _dotLength)
    {
        for (int i = 0; i < _dotLength; i++)
        {
            health.TookDamage(_dotDamage);
            yield return new WaitForSeconds(1);
        }

        dotEffect.SetActive(false);
    }
    #endregion

    //Ability Effects

    #region Containment Zone
    public void ActivateContainmentZone(float length)
    {
        containmentZone.SetActive(true);
        StartCoroutine(ContainmentZone(length));
    }

    IEnumerator ContainmentZone(float length)
    {
        yield return new WaitForSeconds(length);
        containmentZone.SetActive(false);
    }
    #endregion
}
