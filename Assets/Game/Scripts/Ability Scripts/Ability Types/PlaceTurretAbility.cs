using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTurretAbility : Ability
{
    public Turret turret;
    public LayerMask placementLayer;
    public int initialDamage;
    public List<Turret> activeTurrets;

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

            Turret newTurret = Instantiate(turret, position, rotation);
            newTurret.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);
            newTurret.Initialize(this);
            newTurret.tag = refManager.friendlyTag.ToString();
            newTurret.gameObject.layer = LayerMask.NameToLayer(refManager.friendlyTag.ToString());
            activeTurrets.Add(newTurret);

            for (int i = 0; i < GetActiveModules().Count; i++)
                newTurret.SetModule(GetActiveModules()[i]);

            RemoveModules();

            TriggerCooldown();
        }
    }

    public override void ModuleActivated(ModuleAbility module)
    {
        base.ModuleActivated(module);
        print("Turret Placed Module Activated");
        if (abilityType == AbilityType.Turret)
            for(int i = 0; i < activeTurrets.Count; i++)
                for (int j = 0; j < GetActiveModules().Count; j++)
                {
                    activeTurrets[i].ModuleActivated(module);
                    activeTurrets[i].SetModule(GetActiveModules()[j]);
                }
    }

    public void RemoveTurret(Turret turretToRemove)
    {
        if (activeTurrets.Contains(turretToRemove))
            activeTurrets.Remove(turretToRemove);
    }
}
