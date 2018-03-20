using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEncounter : MonoBehaviour
{
    public GameObject encounterObject;
    public int spawnAmount;

    Encounter encounter;

    void Start ()
    {
        encounter = GetComponent<Encounter>();
        Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5,5));
        GameObject testSubject =Instantiate(encounterObject, spawnPosition, Quaternion.identity);
        testSubject.GetComponent<AIReferenceManager>().encounter = encounter;

        encounter.encounters.Add(testSubject);

        spawnAmount = testSubject.GetComponent<AIReferenceManager>().currentChallengeTier.spawnCount - 1;
        for(int i = 0; i < spawnAmount; i++)
        {
            Vector3 newSpawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            GameObject newTestSubject = Instantiate(encounterObject, spawnPosition, Quaternion.identity);

            newTestSubject.GetComponent<AIReferenceManager>().encounter = encounter;
            encounter.encounters.Add(newTestSubject);
        }
	}
}
