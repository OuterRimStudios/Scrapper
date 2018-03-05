using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float baseSpeed;

    float speed;
    Rigidbody rb;

    bool canAct;
    Coroutine stun;
    Coroutine slow;
    Coroutine cc;
    Coroutine root;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = baseSpeed;
        canAct = true;
    }

    #region Stun
    public void ApplyStun(float stunLength)
    {
        if (stun != null)
            StopCoroutine(stun);

        speed = 0;
        canAct = false;
        print("Stunned " + speed);

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
        speed = baseSpeed;
        print("Stun Removed " + speed);
    }
#endregion
    #region Slow
    public void ApplySlow(float slowAmount)
    {
        if (stun != null)
            StopCoroutine(stun);

        print("Slow Amt " + slowAmount);
        float slowPercentage = slowAmount / 100;
        float newSpeed = (baseSpeed * slowPercentage);

        if (speed - newSpeed >= 0)
            speed -= newSpeed;
        else
            speed = 0;

        print("AI Slowed -- " + " Slow Percentage : " + slowPercentage + " New Speed : " + newSpeed + " Current Speed: " +  speed);

        slow = StartCoroutine(Slowed(slowAmount));
    }

    IEnumerator Slowed(float slowAmount)
    {
        yield return new WaitForSeconds(slowAmount);
        RemoveSlow();
    }

    public void RemoveSlow()
    {
        speed = baseSpeed;
        print("Slow Removed " + speed);
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

        speed = 0;
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

        speed = baseSpeed;
        canAct = true;

        print("CC Removed");
    }
#endregion
    #region Root
    public void ApplyRoot(float rootLength)
    {
        if (root != null)
            StopCoroutine(root);

        speed = 0;
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

        speed = baseSpeed;

        print("Root Removed");
    }
    #endregion
}
