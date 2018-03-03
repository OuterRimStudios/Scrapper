using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject firstPersonCharacter;
    public GameObject thirdPersonCharacter;

    Animator anim;

    bool firstPerson;

    private void Start()
    {
        firstPerson = true;
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    public void SwitchView()
    {
        firstPerson = !firstPerson;

        if (!firstPerson)
        {
            anim.enabled = false;
            firstPersonCharacter.SetActive(true);
            thirdPersonCharacter.SetActive(false);
        }
        else
        {
            anim.enabled = true;
            firstPersonCharacter.SetActive(false);
            thirdPersonCharacter.SetActive(true);
        }
    }
}
