using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementAbility : Ability
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            Vector3 position = hit.point + (hit.normal * .1f);
            Quaternion rotation = Quaternion.LookRotation(hit.normal + (Vector3.forward * 90));

            Trap newTrap = Instantiate(trap, position, rotation);
            newTrap.Initialize(initialDamage, afterEffects);

            for (int i = 0; i < GetActiveModules().Count; i++)
                newTrap.SetModule(GetActiveModules()[i]);
            RemoveModules();

            TriggerCooldown();
        }
    }
}
