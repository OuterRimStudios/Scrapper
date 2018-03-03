using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject firstPersonCharacter;
    public GameObject thirdPersonCharacter;
    public Transform firstPersonSpawnpoint;
    public Transform thirdPersonSpawnpoint;

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
            thirdPersonCharacter.SetActive(false);
            firstPersonCharacter.SetActive(true);
        }
        else
        {
            anim.enabled = true;
            thirdPersonCharacter.SetActive(true);
            firstPersonCharacter.SetActive(false);
        }
    }

    public Transform SpawnPosition()
    {
        if(!firstPerson)
            return thirdPersonSpawnpoint;
        else
            return firstPersonSpawnpoint;
    }
}
