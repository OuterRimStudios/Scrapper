using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHealth : ObjectReference
{
    public Health health;

    private void Awake()
    {
        SetReference(health);
    }
}
