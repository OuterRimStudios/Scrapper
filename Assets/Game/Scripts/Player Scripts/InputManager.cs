﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode jumpKey;
    public KeyCode interactKey;
    public KeyCode toggleViewKey;

    [Space, Header("Ability Keys")]
    public KeyCode abilityOneKey;
    public KeyCode abilityTwoKey;
    public KeyCode abilityThreeKey;
    public KeyCode abilityFourKey;
    public KeyCode abilityFiveKey;

    [Space, Header("Ability Loadout")]
    public List<Ability> abilities = new List<Ability>();
    bool[] abilityActive = new bool[5];
    bool[] abilityDeactive = new bool[5];
    //public Ability abilityOne;
    //public Ability abilityTwo;
    //public Ability abilityThree;
    //public Ability abilityFour;
    //public Ability abilityFive;

    bool jump;
    bool interact;
    bool toggleView;

    //bool abilityOneActive;
    //bool abilityTwoActive;
    //bool abilityThreeActive;
    //bool abilityFourActive;
    //bool abilityFiveActive;

    float moveX;
    float moveY;
    float lookX;
    float lookY;

    PlayerManager playerManager;
    PlayerMovement playerMovement;
    CameraController cameraController;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    private void Update()
    {
        RecieveInput();
        AbilityInput();
        playerMovement.RecieveInput(moveX, moveY, lookX, lookY);
        cameraController.RecieveInput(lookY);

        if (toggleView)
        {
            playerManager.SwitchView();
            cameraController.SwitchView();
        }

        if (jump)
            playerMovement.Jump();
    }

    void AbilityInput()
    {
        for (int i = 0; i < abilities.Count; i ++)
        {
            if (abilities[i] && abilities[i].CanShoot() && abilityActive[i])
                abilities[i].ActivateAbility();
            else if(abilities[i] && abilityDeactive[i])
                abilities[i].DeactivateAbility();
        }
        //if (abilityOne && abilityOne.CanShoot() && abilityOneActive)
        //    abilityOne.ActivateAbility();

        //if (abilityTwo && abilityTwo.CanShoot() && abilityTwoActive)
        //    abilityTwo.ActivateAbility();

        //if (abilityThree && abilityThree.CanShoot() && abilityThreeActive)
        //    abilityThree.ActivateAbility();

        //if (abilityFour && abilityFour.CanShoot() && abilityFourActive)
        //    abilityFour.ActivateAbility();

        //if (abilityFive && abilityFive.CanShoot() && abilityFiveActive)
        //    abilityFive.ActivateAbility();
    }

    private void RecieveInput()
    {
        jump = Input.GetKeyDown(jumpKey);
        interact = Input.GetKey(interactKey);
        toggleView = Input.GetKeyDown(toggleViewKey);

        abilityActive[0] = Input.GetKeyDown(abilityOneKey);
        abilityActive[1] = Input.GetKeyDown(abilityTwoKey);
        abilityActive[2] = Input.GetKeyDown(abilityThreeKey);
        abilityActive[3] = Input.GetKeyDown(abilityFourKey);
        abilityActive[4] = Input.GetKeyDown(abilityFiveKey);

        abilityDeactive[0] = Input.GetKeyUp(abilityOneKey);
        abilityDeactive[1] = Input.GetKeyUp(abilityTwoKey);
        abilityDeactive[2] = Input.GetKeyUp(abilityThreeKey);
        abilityDeactive[3] = Input.GetKeyUp(abilityFourKey);
        abilityDeactive[4] = Input.GetKeyUp(abilityFiveKey);

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        lookX = Input.GetAxis("Mouse X");
        lookY = Input.GetAxis("Mouse Y");
    }
}
