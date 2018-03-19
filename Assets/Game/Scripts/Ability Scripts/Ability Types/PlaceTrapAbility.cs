using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrapAbility : Ability
{
    public Trap trap;
    public LayerMask placementLayer;
    public int initialDamage;

    Camera myCamera;

    private void Awake()
    {
        myCamera = Camera.main;
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
  
        Vector3 position = refManager.transform.position + refManager.transform.forward * 2;

        Trap newTrap = Instantiate(trap, position, Quaternion.identity);
        newTrap.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

        for (int i = 0; i < GetActiveModules().Count; i++)
            newTrap.SetModule(GetActiveModules()[i]);
        RemoveModules();

        TriggerCooldown();
    }
}
