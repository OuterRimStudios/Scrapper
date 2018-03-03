using System.Collections;
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
    public Ability abilityOne;
    public Ability abilityTwo;
    public Ability abilityThree;
    public Ability abilityFour;
    public Ability abilityFive;

    bool jump;
    bool interact;
    bool toggleView;

    bool abilityOneActive;
    bool abilityTwoActive;
    bool abilityThreeActive;
    bool abilityFourActive;
    bool abilityFiveActive;

    float moveX;
    float moveY;
    float lookX;
    float lookY;

    PlayerManager playerManager;
    PlayerMovement playerMovement;
    CameraController cameraController;

    private void Awake()
    {
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
        if (abilityOne && !abilityOne.OnCooldown() && abilityOneActive)
            abilityOne.ActivateAbility();

        if (abilityTwo && !abilityTwo.OnCooldown() && abilityTwoActive)
            abilityTwo.ActivateAbility();

        if (abilityThree && !abilityThree.OnCooldown() && abilityThreeActive)
            abilityThree.ActivateAbility();

        if (abilityFour && !abilityFour.OnCooldown() && abilityFourActive)
            abilityFour.ActivateAbility();

        if (abilityFive && !abilityFive.OnCooldown() && abilityFiveActive)
            abilityFive.ActivateAbility();
    }

    private void RecieveInput()
    {
        jump = Input.GetKeyDown(jumpKey);
        interact = Input.GetKey(interactKey);
        toggleView = Input.GetKeyDown(toggleViewKey);

        abilityOneActive = Input.GetKey(abilityOneKey);
        abilityTwoActive = Input.GetKey(abilityTwoKey);
        abilityThreeActive = Input.GetKey(abilityThreeKey);
        abilityFourActive = Input.GetKey(abilityFourKey);
        abilityFiveActive = Input.GetKey(abilityFiveKey);

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        lookX = Input.GetAxis("Mouse X");
        lookY = Input.GetAxis("Mouse Y");
    }
}
