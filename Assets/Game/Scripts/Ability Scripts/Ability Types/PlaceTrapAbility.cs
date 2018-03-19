using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrapAbility : Ability
{
    public Trap trap;
    public LayerMask placementLayer;
    public int initialDamage;
    public bool placeAtRange;

    Camera myCamera;

    private void Awake()
    {
        myCamera = Camera.main;
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Trap newTrap = new Trap();
        if (placeAtRange)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, placementLayer))
            {
                Vector3 position = hit.point + (hit.normal * .1f);
                Quaternion rotation = Quaternion.LookRotation(hit.normal + (Vector3.forward * 90));

                newTrap = Instantiate(trap, position, rotation);
            }
        }
        else
        {
            Vector3 position = refManager.transform.position + refManager.transform.forward * 2;
            position.y = 0;
            newTrap = Instantiate(trap, position, Quaternion.identity);
        }

        newTrap.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

        for (int i = 0; i < GetActiveModules().Count; i++)
            newTrap.SetModule(GetActiveModules()[i]);

        RemoveModules();
        TriggerCooldown();
    }
}
