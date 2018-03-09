using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffects : MonoBehaviour
{
    public float baseSpeed;
    public List<Transform> waypoints;
    Vector3 targetPos;
    int currentWaypoint;
    
    Rigidbody rb;

    bool canAct;
    Coroutine stun;
    Coroutine slow;
    Coroutine cc;
    Coroutine root;

    NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
        canAct = true;
    }

    private void Update()
    {
        /*if(canAct)
        {
            if (waypoints.Count > 0)
            {
                if (targetPos != waypoints[currentWaypoint].position)
                {
                    targetPos = waypoints[currentWaypoint].position;
                }
                else
                {
                    float dist = Vector3.Distance(transform.position, targetPos);

                    if (dist > .6f)
                    {
                        Vector3 targetPosition = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                        transform.LookAt(targetPosition);
                        transform.position += transform.forward * speed * Time.deltaTime;
                    }
                        //transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
                    else
                    {
                        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                    }
                }
            }
        }*/
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
}
