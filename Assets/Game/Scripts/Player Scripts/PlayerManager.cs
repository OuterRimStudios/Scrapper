using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject firstPersonCharacter;
    public GameObject thirdPersonCharacter;
    public Transform firstPersonSpawnpoint;
    public Transform thirdPersonSpawnpoint;

    public GameObject gun;
    public Transform firstPersonGunPosition;
    public Transform thirdPersonGunPosition;

    Animator anim;

    bool firstPerson;

    private void Awake()
    {
        firstPerson = true;
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    public void SwitchView()
    {
        firstPerson = !firstPerson;

        if (firstPerson)
        {
            anim.enabled = false;
            thirdPersonCharacter.SetActive(false);
            UpdateGunPos(firstPersonGunPosition);
            firstPersonCharacter.SetActive(true);
        }
        else
        {
            anim.enabled = true;
            firstPersonCharacter.SetActive(false);
            UpdateGunPos(thirdPersonGunPosition);
            thirdPersonCharacter.SetActive(true);
        }
    }

    public Transform SpawnPosition()
    {
        if (!firstPerson)
            return thirdPersonSpawnpoint;
        else
            return firstPersonSpawnpoint;
    }

    void UpdateGunPos(Transform _parent)
    {
        gun.transform.SetParent(_parent);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;
    }
}