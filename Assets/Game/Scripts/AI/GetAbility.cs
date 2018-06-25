using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAbility : ObjectReference
{
    public Ability ability;

    private void Awake()
    {
        SetReference(ability);
    }
}
