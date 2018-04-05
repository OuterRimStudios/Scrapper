using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class InputManager : MonoBehaviour
{
    public KeyCode jumpKey;
    public KeyCode interactKey;
    public KeyCode toggleViewKey;
    public KeyCode loadoutMenuKey;
    public KeyCode pauseKey;

    [Space, Header("Ability Keys")]
    public KeyCode abilityOneKey;
    public KeyCode abilityTwoKey;
    public KeyCode abilityThreeKey;
    public KeyCode abilityFourKey;
    public KeyCode abilityFiveKey;

    [Space, Header("Ability Loadout")]
    public AbilityDisplayArea abilityDisplayArea;
    public List<Ability> abilities = new List<Ability>();
    public List<Image> abilitySlots;
    public List<Text> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();
    public List<ActiveAbilitySlot> activeAbilitySlots;

    [Space, Header("Menus")]
    public GameObject loadoutMenu;
    public GameObject hud;
    public GameObject pauseMenu;

    public bool hideCursor = true;

    bool[] abilityOnCooldown = new bool[5];
    bool[] abilityActive = new bool[5];
    bool[] abilityDeactive = new bool[5];

    bool jump;
    bool interact;
    bool toggleView;
    bool toggleLoadoutMenu;
    bool pause;
    public static bool canAct;

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
        Application.targetFrameRate = 60;
        player = ReInput.players.GetPlayer(0);
        canAct = true;
        playerRefManager = GetComponent<PlayerReferenceManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponentInChildren<CameraController>();

        for(int i = 0; i < abilities.Count; i++)
        {
            activeAbilitySlots[i].SetAbilitySlot(abilities[i]);
            if(abilityDisplayArea.abilityLoadoutOptions.Contains(activeAbilitySlots[i].abilityInSlot))
            {
                int index = abilityDisplayArea.abilityLoadoutOptions.IndexOf(activeAbilitySlots[i].abilityInSlot);
                ActiveAbilitySlot abilitySlot = abilityDisplayArea.abilitySlots[index];
                abilitySlot.AbilityActive(true);
            }
        }

        UpdateAbilities(-1);

        for(int i = 0; i < 5; i++)
            cooldownQueues.Add(new Queue<float>());

        if(hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //pass in -1 if not changing abilities
    public void UpdateAbilities(int abilitySlotIndex)
    {
        if(abilitySlotIndex != -1)
        {
            abilities[abilitySlotIndex].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
            if(abilityDisplayArea.abilityLoadoutOptions.Contains(abilities[abilitySlotIndex]))
            {
                int slotIndex = abilityDisplayArea.abilityLoadoutOptions.IndexOf(abilities[abilitySlotIndex]);
                abilityDisplayArea.abilitySlots[slotIndex].AbilityActive(false);
            }
            abilities[abilitySlotIndex] = abilityDisplayArea.currentActiveAbilitySlot.abilityInSlot;
        }

        for(int i = 0; i < abilities.Count; i++)
        {
            abilities[i].OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
            abilitySlots[i].sprite = abilities[i].abilityIcon;

            if(abilities[i].abilityCharges > 1)
            {
                abilityCharges[i].text = abilities[i].abilityCharges.ToString();
                abilityCharges[i].enabled = true;
            }
        }
    }

    private void Update()
    {
        RecieveInput();

        if(pause)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            if(pauseMenu.activeSelf)
                pauseMenu.SetActive(false);

            AbilityInput();
            CheckCooldowns();
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
        for(int i = 0; i < abilities.Count; i++)
        {
            if(abilities[i] && abilities[i].CanShoot() && abilityActive[i])
            {
                abilities[i].ActivateAbility();
                abilityCharges[i].text = abilities[i].charges.ToString();

                if(abilities[i].charges >= 0)
                {
                    cooldownQueues[i].Enqueue(Time.time);
                }
            }
            else if(abilities[i] && abilityDeactive[i])
                abilities[i].DeactivateAbility();
        }
    }

    void UpdateAbiltyCharges(Ability ability)
    {
        if(abilities.Contains(ability))
        {
            int abilityIndex = abilities.IndexOf(ability);
            abilityCharges[abilityIndex].text = ability.charges.ToString();
        }
    }

    void CheckCooldowns()
    {
        if(cooldownQueues.Count <= 0) return;

        for(int j = 0; j < abilities.Count; j++)
        {
            Cooldown(j);
        }

        for(int i = 0; i < cooldownQueues.Count; i++)
        {
            if(cooldownQueues[i].Count > 0)
                remainingTime = ((cooldownQueues[i].Peek() + abilities[i].abilityCooldown) - Time.time) / abilities[i].abilityCooldown;

            if(remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    void Cooldown(int abilityIndex)
    {
        if(cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex].Peek() + abilities[abilityIndex].abilityCooldown) - Time.time) / abilities[abilityIndex].abilityCooldown;
            abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if(_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
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

        if(!pause && !SpawnManager.waveActive && player.GetButtonDown("Loadout"))
            toggleLoadoutMenu = !toggleLoadoutMenu;

        if(SpawnManager.waveActive && toggleLoadoutMenu)
        {
            canAct = true;
            toggleLoadoutMenu = false;
        }

        if(pause || toggleLoadoutMenu)
            canAct = false;
        else
            canAct = true;

        if(!canAct) return;
        jump = player.GetButtonDown("Jump");
        interact = player.GetButtonDown("Interact");

        abilityActive[0] = player.GetButtonDown("AbilityOne");
        abilityActive[1] = player.GetButtonDown("AbilityTwo");
        abilityActive[2] = player.GetButtonDown("AbilityThree");
        abilityActive[3] = player.GetButtonDown("AbilityFour");
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
        pause = _pause;
    }

    public void ToggleLoadoutMenu(bool _toggleLoadoutMenu)
    {
        toggleLoadoutMenu = _toggleLoadoutMenu;
    }
}