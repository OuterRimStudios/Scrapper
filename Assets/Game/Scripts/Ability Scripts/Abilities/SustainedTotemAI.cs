using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedTotemAI : Ability {

    public Sustained beamPrefab;
    public float effectLength;
    public int initialDamage;

    [HideInInspector] public List<Sustained> beams = new List<Sustained>();
	List<Transform> availableSpawnPositions = new List<Transform>();

    protected override void Start()
    {
        base.Start();
		AIReferenceManager aiRefManager = (AIReferenceManager)refManager;
		ResetSpawnpoints();
        for (int i = 0; i < aiRefManager.currentChallengeTier.spawnCount; i++)
        {
            beams.Add(Instantiate(beamPrefab));
			Transform spawnPosition = GetSpawnPosition();
			UpdateTransform(beams[i].transform, spawnPosition);
            beams[i].Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects, spawnPosition);
            beams[i].gameObject.SetActive(false);
        }
    }

    public override void VisualOnActivate()
    {
		for (int i = 0; i < beams.Count; i++)
        {
            beams[i].SetTarget(refManager.targetManager.GetClosestTarget(transform.position, refManager.enemyTag.ToString()), false);
            beams[i].gameObject.SetActive(true);
        }
        //AIHealth health = (AIHealth)refManager.health;
        //health.SetLimbsActive(true);
        //refManager.animManager.Attack();
    }

    public override void VisualOnDeactivate()
    {
        //AIHealth health = (AIHealth)refManager.health;
        //health.SetLimbsActive(false);
        //refManager.animManager.StopAttack();
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }

        isCharging = false;         //Interupts Attack phase if using attack decision
        isFiring = false;

        VisualOnDeactivate();
        //UpdateTransform();
        TriggerCooldown();
    }

	protected override IEnumerator Charge()
	{
		isCharging = true;
		yield return new WaitForSeconds(chargeTime);
		isCharging = false;

		if (!isFiring)
            StartCoroutine(Firing());
	}

    IEnumerator Firing()
    {
        isFiring = true;
        VisualOnActivate();
        yield return new WaitForSeconds(effectLength);
        DeactivateAbility();
        isFiring = false;
    }

	void UpdateTransform(Transform beam, Transform spawnPosition)
    {
		beam.SetParent(spawnPosition);
		beam.position = Vector3.zero;
		beam.position = spawnPosition.position;
		beam.rotation = spawnPosition.rotation;
    }

	Transform GetSpawnPosition()
    {
        int randomLocation = Random.Range(0, availableSpawnPositions.Count - 1);
        Transform spawnPoint = availableSpawnPositions[randomLocation];
        availableSpawnPositions.Remove(spawnPoint);
        return spawnPoint;
    }

    void ResetSpawnpoints()
    {
        availableSpawnPositions.Clear();
        foreach (Transform point in refManager.SpawnPosition())
        {
            availableSpawnPositions.Add(point);
        }
    }
}