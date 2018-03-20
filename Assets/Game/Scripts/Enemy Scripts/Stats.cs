using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Stats")]
public class Stats : ScriptableObject
{
    public float attackFrequency;
    public float attackRadius;
    public float attackRange;
    public float lookRadius;
    public float lookRange;
    public float moveRange;
    public float personalSpaceRange;
}
