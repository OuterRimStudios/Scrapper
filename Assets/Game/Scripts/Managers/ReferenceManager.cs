using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {

    public AnimationManager animManager;
    [SerializeField] protected Transform[] abilitySpawnPoints;
    public Health health;
    protected Rigidbody rigidBody;
    protected StatusEffects statusEffects;
    public Stats stats;
    public TargetManager targetManager;
    public LayerMask boundLayer;

    [TagSelector]
    public List<string> exclusionTags;

    public enum Tag
    {
        Enemy,
        Friendly
    }

    [Tooltip("Tag of the friendly faction in relation to this object")]
    public Tag friendlyTag;
    [Tooltip("Tag of the enemy faction in relation to this object")]
    public Tag enemyTag;

    protected virtual void Awake()
    {
        animManager = GetComponent<AnimationManager>();
        health = GetComponent<Health>();
        rigidBody = GetComponent<Rigidbody>();
        targetManager = TargetManager.instance;
        targetManager.AddTarget(gameObject, friendlyTag.ToString());
    }

    public virtual void Start()
    {

    }

    public Transform[] SpawnPosition()
    {
        return abilitySpawnPoints;
    }
}
