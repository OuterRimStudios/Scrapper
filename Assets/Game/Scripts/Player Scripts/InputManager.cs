using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class InputManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject radialMenu;
    public float radialMenuTimeScale = 0.5f;
    public GameObject hud;
    public GameObject pauseMenu;
    public Text loadoutMenuWaveActiveText;

    public bool hideCursor = true;

    bool[] abilityOnCooldown = new bool[5];
    bool abilityActive;
    bool abilityDeactive;

    bool jump;
    bool interact;
    bool toggleView;
    bool toggleRadialMenu;
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
    SpawnManager spawnManager;

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
        if(GameObject.Find("GameManager").GetComponent<SpawnManager>())
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();

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

            if (spawnManager && spawnManager.isActiveAndEnabled && !SpawnManager.waveActive && interact)
                spawnManager.PlayerReady();

            AbilityInput();
            playerMovement.RecieveInput(moveX, moveY, lookX, lookY);
            cameraController.RecieveInput(lookY);

            if(toggleView)
            {
                playerRefManager.SwitchView();
                cameraController.SwitchView();
            }

            if(toggleRadialMenu)
            {
                canShoot = false;
                hud.SetActive(false);
                radialMenu.SetActive(true);
                Time.timeScale = radialMenuTimeScale;
            }
            else
            {
                canShoot = true;
                hud.SetActive(true);
                radialMenu.SetActive(false);
                Time.timeScale = 1;
            }

            if(jump)
                playerMovement.Jump();
        }

        if(pause)
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
                Cursor.visible = true;
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
            AbilityManager.instance.abilityCharges[0].text = AbilityManager.instance.currentAbility.charges.ToString();

            if(AbilityManager.instance.currentAbility.charges >= 0)
            {
                AbilityManager.instance.cooldownQueues[0].Enqueue(Time.time);
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
            if (pause)
                pause = false;

            toggleRadialMenu = !toggleRadialMenu;
        }

        if(pause)
        {
            acceptInput = false;
            canShoot = false;
            PlayerMovement.canAct = false;
            CameraController.canAct = false;
        }
        else
        {
            acceptInput = true;
            PlayerMovement.canAct = true;
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
        if(spawnManager && !SpawnManager.waveActive)
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