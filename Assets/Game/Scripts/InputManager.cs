using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode jumpKey;
    public KeyCode interactKey;
    public KeyCode toggleViewKey;

    [Space, Header("Abilities")]
    public KeyCode abilityOneKey;
    public KeyCode abilityTwoKey;
    public KeyCode abilityThreeKey;
    public KeyCode abilityFourKey;
    public KeyCode abilityFiveKey;

    bool jump;
    bool interact;
    bool toggleView;

    bool abilityOne;
    bool abilityTwo;
    bool abilityThree;
    bool abilityFour;
    bool abilityFive;

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

    private void RecieveInput()
    {
        jump = Input.GetKeyDown(jumpKey);
        interact = Input.GetKey(interactKey);
        toggleView = Input.GetKeyDown(toggleViewKey);

        abilityOne = Input.GetKey(abilityOneKey);
        abilityTwo = Input.GetKey(abilityTwoKey);
        abilityThree = Input.GetKey(abilityThreeKey);
        abilityFour = Input.GetKey(abilityFourKey);
        abilityFive = Input.GetKey(abilityFiveKey);

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        lookX = Input.GetAxis("Mouse X");
        lookY = Input.GetAxis("Mouse Y");
    }
}
