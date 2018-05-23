using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSpawner : MonoBehaviour
{
    public List<Encount> possibleEncounters;
    public List<Transform> spawnPoints;

    Encount randomEncounter;

    bool encounterSpawned;

    void Start()
    {
        randomEncounter = possibleEncounters[Random.Range(0, possibleEncounters.Count)];
    }

    public void SpawnEncounter()
    {
        randomEncounter.SpawnEcnounters(spawnPoints);
        encounterSpawned = true;
    }

    public void ResetEncounter()
    {
        encounterSpawned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Friendly"))
        {
            if (encounterSpawned) return;
            SpawnEncounter();
        }
    }
}
