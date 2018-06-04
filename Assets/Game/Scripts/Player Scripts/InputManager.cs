using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class InputManager : MonoBehaviour
{
    [Header("Menus")]
    public RMF_RadialMenu radialMenu;
    public float radialMenuTimeScale = 0.5f;
    public GameObject hud;
    public GameObject pauseMenu;
    public AbilityDisplayArea abilityDisplayArea;
    public Text loadoutMenuWaveActiveText;

    public bool hideCursor = true;

    bool[] abilityOnCooldown = new bool[5];
    bool abilityActive;
    bool abilityDeactive;

    bool jump;
    bool interact;
    bool toggleView;
    bool toggleRadialMenu;
    bool toggleAbilityDisplayArea;
    bool pause;
    bool turningOffText;
    public static bool acceptInput;
    bool canShoot;

    float moveX;
    float moveY;
    float lookX;
    float lookY;
    float remainingTime;

    PlayerReferenceManager playerRefManager;
    PlayerMovement playerMovement;
    CameraController cameraController;

    Player player;

    private void Awake()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        player = ReInput.players.GetPlayer(0);
        acceptInput = true;
        canShoot = true;
        playerRefManager = GetComponent<PlayerReferenceManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponentInChildren<CameraController>();

        playerRefManager.InitializeLookAt();

        if(hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if(playerRefManager.health.isDead)
            return;

        RecieveInput();

        if(pause)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }

            AbilityInput();
            playerMovement.RecieveInput(moveX, moveY, lookX, lookY);

            if(!toggleRadialMenu)
                cameraController.RecieveInput(lookY);

            if (interact)
            {
                toggleAbilityDisplayArea = !toggleAbilityDisplayArea;
                abilityDisplayArea.gameObject.SetActive(toggleAbilityDisplayArea);

                if (abilityDisplayArea.gameObject.activeInHierarchy)
                    abilityDisplayArea.Initialize(AbilityManager.instance.equippedAbilities);
            }

            if(toggleView)
            {
                playerRefManager.SwitchView();
                cameraController.SwitchView();
            }

            if(toggleRadialMenu)
            {
                canShoot = false;
                hud.SetActive(false);
                radialMenu.gameObject.SetActive(true);
                Time.timeScale = radialMenuTimeScale;
            }
            else
            {
                canShoot = true;
                hud.SetActive(true);
                radialMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

            if(jump)
                playerMovement.Jump();
        }

        if(pause || toggleAbilityDisplayArea)
        {
            if(!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if(toggleRadialMenu)
        {
            if(!Cursor.visible)
            {
#if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.None;
#else
                Cursor.lockState = CursorLockMode.Confined;
#endif
                Cursor.visible = false;
            }
        }
        else if(!pause && !toggleRadialMenu)
        {
            if(Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void AbilityInput()
    {
        if(!acceptInput || !canShoot) return;
        if(AbilityManager.instance.currentAbility && AbilityManager.instance.currentAbility.CanShoot() && abilityActive)
        {
            playerMovement.Sprint(false);
            AbilityManager.instance.currentAbility.ActivateAbility();
            AbilityManager.instance.abilityCharges[AbilityManager.instance.CurrentAbilityIndex].text = AbilityManager.instance.currentAbility.charges.ToString();

            if(AbilityManager.instance.currentAbility.charges >= 0)
            {
                AbilityManager.instance.cooldownQueues[AbilityManager.instance.CurrentAbilityIndex].Add(Time.time);
            }
        }
        else if(AbilityManager.instance.currentAbility && abilityDeactive)
            AbilityManager.instance.currentAbility.DeactivateAbility();
    }

    private void RecieveInput()
    {
        toggleView = player.GetButtonDown("ToggleView");
        if(player.GetButtonDown("Pause"))
        {
            if(toggleRadialMenu)
                toggleRadialMenu = false;
            else
                pause = !pause;
        }

        if(player.GetButton("AbilityMenu"))
        {
            if(pause)
                pause = false;

            toggleRadialMenu = true;
        }
        else
        {
            radialMenu.ExecuteSelectedButton();
            toggleRadialMenu = false;
        }

        if(pause)
        {
            acceptInput = false;
            canShoot = false;
            PlayerMovement.canMove = false;
            PlayerMovement.canRotate = false;
            CameraController.canAct = false;
        }
        else if(toggleRadialMenu)
        {
            CameraController.canAct = false;
            PlayerMovement.canRotate = false;
        }
        else if(toggleAbilityDisplayArea)
        {
            canShoot = false;
            PlayerMovement.canMove = false;
            PlayerMovement.canRotate = false;
            CameraController.canAct = false;
        }
        else
        {
            acceptInput = true;
            PlayerMovement.canMove = true;
            PlayerMovement.canRotate = true;
            CameraController.canAct = true;
        }

        if(!acceptInput) return;

        if(!playerMovement.hovering)
            jump = player.GetButtonDown("Jump");
        else
            jump = player.GetButton("Jump");

        interact = player.GetButtonDown("Interact");

        if(AbilityManager.instance.currentAbility.abilityInput == Ability.AbilityInput.GetButton)
            abilityActive = player.GetButton("ActivateAbility");
        else
            abilityActive = player.GetButtonDown("ActivateAbility");

        abilityDeactive = player.GetButtonUp("ActivateAbility");

        moveX = player.GetAxis("MoveHorizontal");
        moveY = player.GetAxis("MoveVertical");

        lookX = player.GetAxis("LookHorizontal");
        lookY = player.GetAxis("LookVertical");
    }

    public void TogglePause(bool _pause)
    {
        pause = _pause;
    }

    public void ToggleLoadoutMenu(bool _toggleLoadoutMenu)
    {
        toggleRadialMenu = _toggleLoadoutMenu;
    }

    IEnumerator TurnOffText()
    {
        yield return new WaitForSecondsRealtime(2f);
        loadoutMenuWaveActiveText.enabled = false;
        turningOffText = false;
    }
}