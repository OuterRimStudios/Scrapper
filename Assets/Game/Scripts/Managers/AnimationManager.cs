using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimatorActive(bool activeState)
    {
        if(anim)
        anim.enabled = activeState;
    }

    public void Attack()
    {
        anim.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        anim.SetBool("Attack", false);
    }
}
