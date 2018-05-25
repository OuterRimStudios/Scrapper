using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableDoor : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void ChangeLockState(bool state)
    {
        anim.SetBool("Close", state);
    }
}
