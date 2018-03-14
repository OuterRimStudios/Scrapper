using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI : MonoBehaviour
{
    public AIReferenceManager refManager;

    public Transform chaseTarget;
    public float baseSpeed;
    public bool updateTarget;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public bool attacking;
    [HideInInspector] public Vector3 walkPos;
    [HideInInspector] public Vector3 previousPos;
    [HideInInspector] public float speed;

    WaitForSeconds attackFrequency;

    public float updateTargetFrequency;                                      //How often the AI will update it's state
    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingTarget;                                                     //Checks if it's time to update the state
    bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes

    Rigidbody rb;

    private void Awake()
    {
        walkPos = transform.position;
        previousPos = transform.position;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackFrequency = new WaitForSeconds(1);
        waitTime = new WaitForSeconds(updateTargetFrequency);

        speed = baseSpeed;
    }

    void Update()
    {
        if (!aiActive) return;

        if (!updateTarget) return;
        if (!updatingTarget)
        {
            updatingTarget = true;
            StartCoroutine(UpdateTarget());
        }
    }

    public void Move()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    public void SetDestination(Vector3 position)
    {
        walkPos = new Vector3(position.x, transform.position.y, position.z);
    }

    public void SetDestination(Transform target)
    {
        if(target)
            walkPos = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

    IEnumerator UpdateTarget()
    {
        chaseTarget = refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString());
        SetDestination(chaseTarget);
        yield return waitTime;
        updatingTarget = false;
    }

    public IEnumerator Attack()
    {
        yield return attackFrequency;
        attacking = false;
    }

    public void InteruptAgent()
    {
        StopAgent();
        refManager.aiAbility.DeactivateAbility();
    }

    public void StopAgent()
    {
        refManager.stateController.aiActive = false;
        aiActive = false;
    }

    public void StartAgent()
    {
        refManager.stateController.aiActive = true;
        aiActive = true;
    }
}
