﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceManager : ReferenceManager
{
    public GameObject firstPersonCharacter;
    public GameObject thirdPersonCharacter;
    public GameObject gun;
    public Transform firstPersonGunPosition;
    public Transform thirdPersonGunPosition;

    List<LookAtCenter> barrelLookAt = new List<LookAtCenter>();

    public static bool firstPerson;

    protected override void Awake()
    {
        base.Awake();
        SetView(firstPerson);
    }

    public void InitializeLookAt()
    {
        for (int i = 0; i < abilitySpawnPoints.Length; i++)
        {
            barrelLookAt.Add(abilitySpawnPoints[i].GetComponent<LookAtCenter>());
        }
    }

    public void SetView(bool _firstPerson)
    {
        if(_firstPerson)
        {
            animManager.SetAnimatorActive(false);
            thirdPersonCharacter.SetActive(false);
            UpdateGunPos(firstPersonGunPosition);
            firstPersonCharacter.SetActive(true);
        }
        else
        {
            animManager.SetAnimatorActive(true);
            firstPersonCharacter.SetActive(false);
            UpdateGunPos(thirdPersonGunPosition);
            thirdPersonCharacter.SetActive(true);
        }
    }

    public void SwitchView()
    {
        firstPerson = !firstPerson;

        SetView(firstPerson);
    }

    void UpdateGunPos(Transform _parent)
    {
        gun.transform.SetParent(_parent);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;
    }

    public void CheckLookAt(Ability ability, int abilityIndex)
    {
        if (ability.dontLookAtCenter)
        {
            barrelLookAt[abilityIndex].enabled = false;
            abilitySpawnPoints[abilityIndex].rotation = Quaternion.Euler(Vector3.zero);
        }
        else
            barrelLookAt[abilityIndex].enabled = true;
    }
}
