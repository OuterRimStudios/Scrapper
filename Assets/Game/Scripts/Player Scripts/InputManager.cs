using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class InputManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject loadoutMenu;
    public GameObject hud;
    public GameObject pauseMenu;
    public Text loadoutMenuWaveActiveText;

    public bool hideCursor = true;

    bool[] abilityOnCooldown = new bool[5];
    bool[] abilityActive = new bool[5];
    bool[] abilityDeactive = new bool[5];

    bool jump;
    bool interact;
    bool toggleView;
    bool toggleLoadoutMenu;
    bool pause;
    bool turningOffText;
    public static bool canAct;

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
        canAct = true;
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

            if(toggleLoadoutMenu)
            {
                hud.SetActive(false);
                loadoutMenu.SetActive(true);
            }
            else
            {
                hud.SetActive(true);
                loadoutMenu.SetActive(false);
            }

            if(jump)
                playerMovement.Jump();
        }

        if(pause || toggleLoadoutMenu)
            hideCursor = false;
        else if(!pause && !toggleLoadoutMenu)
            hideCursor = true;

        if(hideCursor)
        {
            if(Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            if(!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void AbilityInput()
    {
        if(!canAct) return;
        for(int i = 0; i < AbilityManager.instance.currentLoadout.ActiveAbilities.Count; i++)
        {
            if(AbilityManager.instance.currentLoadout.ActiveAbilities[i] && AbilityManager.instance.currentLoadout.ActiveAbilities[i].CanShoot() && abilityActive[i])
            {
                playerMovement.Sprint(false);
                AbilityManager.instance.currentLoadout.ActiveAbilities[i].ActivateAbility();
                AbilityManager.instance.abilityCharges[i].text = AbilityManager.instance.currentLoadout.ActiveAbilities[i].charges.ToString();

                if(AbilityManager.instance.currentLoadout.ActiveAbilities[i].charges >= 0)
                {
                    AbilityManager.instance.cooldownQueues[i].Enqueue(Time.time);
                }
            }
            else if(AbilityManager.instance.currentLoadout.ActiveAbilities[i] && abilityDeactive[i])
                AbilityManager.instance.currentLoadout.ActiveAbilities[i].DeactivateAbility();
        }
    }

    private void RecieveInput()
    {
        toggleView = player.GetButtonDown("ToggleView");
        if(player.GetButtonDown("Pause"))
        {
            if(toggleLoadoutMenu)
                toggleLoadoutMenu = false;
            else
                pause = !pause;
        }

        if(player.GetButtonDown("Loadout"))
        {
            if(spawnManager && SpawnManager.waveActive)
            {
                loadoutMenuWaveActiveText.enabled = true;
                if(!turningOffText)
                {
                    turningOffText = true;
                    StartCoroutine(TurnOffText());
                }
                return;
            }

            if (pause)
                pause = false;

            toggleLoadoutMenu = !toggleLoadoutMenu;
        }

        if(spawnManager && SpawnManager.waveActive && toggleLoadoutMenu)
        {
            canAct = true;
            PlayerMovement.canAct = true;
            CameraController.canAct = true;
            toggleLoadoutMenu = false;
        }

        if(pause || toggleLoadoutMenu)
        {
            canAct = false;
            PlayerMovement.canAct = false;
            CameraController.canAct = false;
        }
        else
        {
            canAct = true;
            PlayerMovement.canAct = true;
            CameraController.canAct = true;
        }

        if(!canAct) return;

        if(!playerMovement.hovering)
            jump = player.GetButtonDown("Jump");
        else
            jump = player.GetButton("Jump");

        interact = player.GetButtonDown("Interact");

        if(AbilityManager.instance.currentLoadout.ActiveAbilities[0].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[0] = player.GetButton("AbilityOne");
        else
            abilityActive[0] = player.GetButtonDown("AbilityOne");

        if (AbilityManager.instance.currentLoadout.ActiveAbilities[1].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[1] = player.GetButton("AbilityTwo");
        else
            abilityActive[1] = player.GetButtonDown("AbilityTwo");

        if (AbilityManager.instance.currentLoadout.ActiveAbilities[2].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[2] = player.GetButton("AbilityThree");
        else
            abilityActive[2] = player.GetButtonDown("AbilityThree");


        if (AbilityManager.instance.currentLoadout.ActiveAbilities[3].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[3] = player.GetButton("AbilityFour");
        else
            abilityActive[3] = player.GetButtonDown("AbilityFour");


        if (AbilityManager.instance.currentLoadout.ActiveAbilities[4].abilityInput == Ability.AbilityInput.GetButton)
            abilityActive[4] = player.GetButton("AbilityFive");
        else
            abilityActive[4] = player.GetButtonDown("AbilityFive");

        abilityDeactive[0] = player.GetButtonUp("AbilityOne");
        abilityDeactive[1] = player.GetButtonUp("AbilityTwo");
        abilityDeactive[2] = player.GetButtonUp("AbilityThree");
        abilityDeactive[3] = player.GetButtonUp("AbilityFour");
        abilityDeactive[4] = player.GetButtonUp("AbilityFive");

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
        if (spawnManager && SpawnManager.waveActive)
        {
            loadoutMenuWaveActiveText.enabled = true;
            if(!turningOffText)
            {
                turningOffText = true;
                StartCoroutine(TurnOffText());
            }
            return;
        }
        else
            toggleLoadoutMenu = _toggleLoadoutMenu;
    }

    IEnumerator TurnOffText()
    {
        yield return new WaitForSecondsRealtime(2f);
        loadoutMenuWaveActiveText.enabled = false;
        turningOffText = false;
    }
}