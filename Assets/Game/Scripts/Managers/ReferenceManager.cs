using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {

    public AnimationManager animManager;
    [SerializeField] protected Transform[] abilitySpawnPoints;
    public GameObject followPointContainer;
    public Health health;
    protected Rigidbody rigidBody;
    protected StatusEffects statusEffects;
    public Stats stats;
    public TargetManager targetManager;
    public LayerMask boundLayer;

    [TagSelector]
    public string[] exclusionTags;

    public enum Tag
    {
        Enemy,
        Friendly
    }

    [Tooltip("Tag of the friendly faction in relation to this object")]
    public Tag friendlyTag;
    [Tooltip("Tag of the enemy faction in relation to this object")]
    public Tag enemyTag;

    //The following followPoint lists are parallel
    public List<Transform> followPoints;
    public List<bool> followPointsFilled;

    protected virtual void Awake()
    {
        animManager = GetComponent<AnimationManager>();
        health = GetComponent<Health>();
        rigidBody = GetComponent<Rigidbody>();
        targetManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TargetManager>();
        targetManager.AddTarget(gameObject, friendlyTag.ToString());
    }

    public virtual void Start()
    {

    }

    public Transform[] SpawnPosition()
    {
        return abilitySpawnPoints;
    }

    public Transform GetFollowPoint()
    {
        if (!followPointsFilled.Contains(false))
        {
            GameObject clone = Instantiate(new GameObject(), followPointContainer.transform);
            int index = followPoints.Count % 8;
            print("Get point: " + index);
            clone.transform.position = followPoints[index].position + followPoints[index].position + followPoints[index].position;
            followPoints.Add(clone.transform);
            followPointsFilled.Add(true);
            return clone.transform;
        }
        else
        {
            for (int i = 0; i < followPointsFilled.Count; i++)
            {
                if (!followPointsFilled[i])
                {
                    followPointsFilled[i] = true;
                    return followPoints[i];
                }
            }

            return null;
        }
    }

    public void RemoveFollowTarget(Transform followPoint)
    {
        if (followPoints.Contains(followPoint))
        {
            int index = followPoints.IndexOf(followPoint);
            print("remove at:" + index);
            followPointsFilled[index] = false;
        }
    }
}