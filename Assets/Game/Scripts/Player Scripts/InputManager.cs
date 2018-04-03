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
    float elapsedTime;
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

                if(abilities[i].charges > 0)
                {
                    cooldownQueues[i].Enqueue(Time.time);
                }
                

                if(!abilityOnCooldown[i])
                {
                    abilityOnCooldown[i] = true;
                    StartCoroutine(Cooldown(i));
                }
            }
            else if (abilities[i] && abilityDeactive[i])
                abilities[i].DeactivateAbility();
        }
    }

    void CheckCooldowns()
    {
        if (cooldownQueues.Count <= 0) return;
        for(int i = 0; i < cooldownQueues.Count; i++)
        {
            if(cooldownQueues[i].Count > 0)
            elapsedTime = Time.time - cooldownQueues[i].Peek();

            if(elapsedTime > abilities[i].abilityCooldown && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    IEnumerator Cooldown(int abilityIndex)
    {
        if (cooldownQueues[abilityIndex].Count > 0)
            remainingTime = (Time.time - cooldownQueues[abilityIndex].Peek()) / abilities[abilityIndex].abilityCooldown;

        for (float i = remainingTime; i >= 0; i -= .1f)
        {
            abilityCooldownProgress[abilityIndex].fillAmount = i;
            yield return new WaitForSeconds(abilities[abilityIndex].abilityCooldown / 10);
            if (cooldownQueues[abilityIndex].Count > 0)
                remainingTime = (Time.time - cooldownQueues[abilityIndex].Peek()) / abilities[abilityIndex].abilityCooldown;
        }
        
        abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        abilityCharges[abilityIndex].text = abilities[abilityIndex].charges.ToString();
        if(abilities[abilityIndex].abilityCharges > 1)
            abilityOnCooldown[abilityIndex] = false;
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