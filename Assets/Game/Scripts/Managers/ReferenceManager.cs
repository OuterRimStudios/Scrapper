using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {

    protected Animator anim;
    [SerializeField] protected Transform[] abilitySpawnPoints;
    protected Health health;
    protected Rigidbody rigidBody;
    protected StatusEffects statusEffects;
    public Stats stats;
    public TargetManager targetManager;
    public LayerMask boundLayer;

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
        anim = GetComponent<Animator>();
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
}
