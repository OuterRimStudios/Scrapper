using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedAbility : Ability {

    public Sustained beamPrefab;
    public int initialDamage;

    List<Sustained> beams = new List<Sustained>();
    GameObject mainCam;

    protected override void Start()
    {
        base.Start();
        mainCam = Camera.main.gameObject;
        for (int i = 0; i < refManager.SpawnPosition().Length; i++)
        {
            beams.Add(Instantiate(beamPrefab));
            UpdateTransform();
            beams[i].Initialize(initialDamage, refManager.enemyTag.ToString(), afterEffects, refManager.SpawnPosition()[i]);
            if(refManager.friendlyTag.ToString() == "Friendly")
                beams[i].SetTarget(mainCam.transform, false, false);
        }
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        //UpdateTransform();

        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].SetTarget(refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString()), false, false);

            beams[i].gameObject.SetActive(true);
        }

        RemoveModules();
    }

    public override void ModuleActivated(ModuleAbility module)
    {
        base.ModuleActivated(module);
        for (int i = 0; i < beams.Count; i++)
        {
            for (int j = 0; j < GetActiveModules().Count; j++)
                beams[i].SetModule(GetActiveModules()[j]);
        }

        RemoveModules();
    }

    public override void DeactivateAbility()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }
        //UpdateTransform();
        TriggerCooldown();
    }

    protected override IEnumerator Charge()
    {
        return base.Charge();
    }

    void UpdateTransform()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].transform.SetParent(refManager.SpawnPosition()[i]);
            beams[i].transform.position = refManager.SpawnPosition()[i].position;
            beams[i].transform.rotation = refManager.SpawnPosition()[i].rotation;
        }
    }
}