using System.Collections.Generic;
using UnityEngine;

public class EncounterSpawner : MonoBehaviour
{
    public List<Encounter> possibleEncounters;
    public List<Transform> spawnPoints;
    public List<LockableDoor> doors;

    public List<GameObject> activeEnemies = new List<GameObject>();

    private Encounter randomEncounter;

    private bool encounterSpawned;
    private bool doorsLocked;

    private void Start()
    {
        if(randomEncounter == null)
            randomEncounter = possibleEncounters[Random.Range(0, possibleEncounters.Count)];

        foreach(Encounter encounter in possibleEncounters)
            encounter.Initialize(this);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ResetEncounter;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ResetEncounter;
    }

    public void SpawnEncounter()
    {
        encounterSpawned = true;
        randomEncounter.SpawnEcnounters(spawnPoints);
        doorsLocked = true;

        for(int i = 0; i < doors.Count; i++)                   // remove later
            doors[i].ChangeLockState(true);
    }

    public void AddEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if(activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            if(activeEnemies.Count <= 0)
            {
                for(int i = 0; i < doors.Count; i++)                  // remove later
                    doors[i].ChangeLockState(false);

                doorsLocked = false;
                PlayerHealth.OnPlayerDied -= ResetEncounter;
            }
        }
    }

    public void ResetEncounter()
    {
        if(!encounterSpawned) return;
        foreach(GameObject go in activeEnemies)
        {
            AIHealth health = go.GetComponent<AIHealth>();
            if(health != null)
                health.DestroyEnemy();
            else
                Destroy(go);
        }

        activeEnemies.Clear();

        for(int i = 0; i < doors.Count; i++)                  // remove later
            doors[i].ChangeLockState(false);

        doorsLocked = false;
        encounterSpawned = false;
    }

    public void ForceClearEncounter()
    {
        if(!encounterSpawned) return;
        foreach(GameObject go in activeEnemies)
        {
            AIHealth health = go.GetComponent<AIHealth>();
            if(health != null)
                health.DestroyEnemy();
            else
                Destroy(go);
        }

        activeEnemies.Clear();

        for(int i = 0; i < doors.Count; i++)                  // remove later
            doors[i].ChangeLockState(false);

        doorsLocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Friendly"))
        {
            if(encounterSpawned) return;
            SpawnEncounter();
        }
    }
}