using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFSetAttackTrigger : ExtraFunctionality
{
    public Animator anim;

    public override void ExtraFunctions()
    {
        print("EXTRA FUNCTIONS ---- >> ATTACK");
        anim.SetTrigger("Attack");
    }
}
