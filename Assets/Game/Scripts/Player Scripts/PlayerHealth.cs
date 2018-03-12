using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Died()
    {
        refManager.targetManager.RemoveFriendly(gameObject);
        base.Died();
    }
}