using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceManager : ReferenceManager {

    public GameObject firstPersonCharacter;
    public GameObject thirdPersonCharacter;
    public GameObject gun;
    public Transform firstPersonGunPosition;
    public Transform thirdPersonGunPosition;

    bool firstPerson;

    protected override void Awake()
    {
        base.Awake();
        firstPerson = true;
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

    void UpdateGunPos(Transform _parent)
    {
        gun.transform.SetParent(_parent);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        gun.transform.localScale = Vector3.one;
    }
}
