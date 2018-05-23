using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEncounter : MonoBehaviour
{
    public GameObject encounterObject;
    public int spawnAmount;
    public bool groupSpawn;
    List<Transform> availableSpawnPositions = new List<Transform>();

    Encounter encounter;
    SpawnManager spawnManager;

    public void SpawnEncounter(List<Transform> spawnPoints)
    {
        if (!groupSpawn)
        {
            spawnManager = SpawnManager.instance;
            ResetSpawnpoints();
        }
        encounter = GetComponent<Encounter>();

        //This is used to figure out how many we need to spawn. This code is NEEDED.
        Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5,5));
        GameObject testSubject =Instantiate(encounterObject, transform.position + spawnPosition, Quaternion.identity);
        testSubject.GetComponent<AIReferenceManager>().encounter = encounter;

        encounter.encounters.Add(testSubject);

        spawnAmount = testSubject.GetComponent<AIReferenceManager>().currentChallengeTier.spawnCount - 1;
        for(int i = 0; i < spawnAmount; i++)
        {
            Vector3 newSpawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            GameObject newTestSubject = null;

            if(groupSpawn)
                newTestSubject = Instantiate(encounterObject, transform.position + spawnPosition, Quaternion.identity);
            else
                newTestSubject = Instantiate(encounterObject, GetSpawnPosition().position, Quaternion.identity);

            newTestSubject.GetComponent<AIReferenceManager>().encounter = encounter;
            encounter.encounters.Add(newTestSubject);
        }
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
        foreach (Transform point in spawnManager.spawnPositions)
        {
            availableSpawnPositions.Add(point);
        }
    }
}
