using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Image> abilitySlots;
    public List<Text> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();
    bool[] abilityOnCooldown = new bool[5];
    bool[] abilityActive = new bool[5];
    bool[] abilityDeactive = new bool[5];

    bool jump;
    bool interact;
    bool toggleView;

    float moveX;
    float moveY;
    float lookX;
    float lookY;
    float remainingTime;

    PlayerReferenceManager playerRefManager;
    PlayerMovement playerMovement;
    CameraController cameraController;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        playerRefManager = GetComponent<PlayerReferenceManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponentInChildren<CameraController>();
        UpdateAbilities();

        for (int i = 0; i < 5; i++)
            cooldownQueues.Add(new Queue<float>());
    }

    public void UpdateAbilities()
    {
        for(int i = 0; i < abilities.Count; i++)
        {
            abilities[i].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
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
        AbilityInput();
        CheckCooldowns();
        playerMovement.RecieveInput(moveX, moveY, lookX, lookY);
        cameraController.RecieveInput(lookY);

        if (toggleView)
        {
            playerRefManager.SwitchView();
            cameraController.SwitchView();
        }

        if (jump)
            playerMovement.Jump();
    }

    void AbilityInput()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] && abilities[i].CanShoot() && abilityActive[i])
            {
                abilities[i].ActivateAbility();
                abilityCharges[i].text = abilities[i].charges.ToString();

                if(abilities[i].charges >= 0)
                {
                    cooldownQueues[i].Enqueue(Time.time);
                }
            }
            else if (abilities[i] && abilityDeactive[i])
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
        if (cooldownQueues.Count <= 0) return;

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