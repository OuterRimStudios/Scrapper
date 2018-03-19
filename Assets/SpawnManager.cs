using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Encounter> encounters;
    public static int currentDifficulty = 0;

    public List<DifficultyRating> difficultyRatings;

    int currentWave;

    List<Encounter> nextEncounters = new List<Encounter>();
    List<string> nextEncounterObjects = new List<string>();
    bool waveActive;
    bool countingDown;

    bool setUpTimer;

    Coroutine timeToSetUp;

    private void Start()
    {
        PrepareWave();
        PlayerReady();

     //   if (currentDifficulty == 1 || currentDifficulty == 2)
            setUpTimer = true;
    }

    public void PlayerReady()
    {
        if (!countingDown)
        {
            countingDown = true;
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        for (int i = 3; i > 0; i--)
        {
            print(i);
            yield return new WaitForSeconds(1);
        }
        countingDown = false;
        StartWave();
    }

    void PrepareWave()
    {
        for (int i = 0; i < difficultyRatings[currentDifficulty].currentEncounterMax; i++)
        {
            int encounterIndex = Random.Range(0, encounters.Count - 1);
            Encounter randomEncounter = encounters[encounterIndex];

            int numberSpawned = 0;
            numberSpawned = CheckEncounters(nextEncounterObjects, randomEncounter.name, numberSpawned);

            if (numberSpawned < randomEncounter.maxEncounters)
            {
                Encounter newEncounter = Instantiate(randomEncounter);

                newEncounter.name = randomEncounter.name;

                nextEncounters.Add(newEncounter);
                nextEncounterObjects.Add(newEncounter.name);

                newEncounter.OnRemoveEncounter += RemoveEncounter;
            }
        }

        foreach (Encounter encounter in nextEncounters)
        {
            encounter.gameObject.SetActive(false);
        }
    }

    void RemoveEncounter(Encounter _encounter)
    {
        if(nextEncounters.Contains(_encounter))
        {
            nextEncounters.Remove(_encounter);
            nextEncounterObjects.Remove(_encounter.name);
            _encounter.OnRemoveEncounter -= RemoveEncounter;
        }
    }

    public void StartWave()
    {
        waveActive = true;
        currentWave++;

        if (timeToSetUp != null)
            StopCoroutine(timeToSetUp);

        foreach (Encounter encounter in nextEncounters)
        {
            encounter.gameObject.SetActive(true);
        }
    }

    int CheckEncounters(List<string> _encounters, string encounterToSpawn, int count)
    {
        if (nextEncounterObjects.Count <= 0)
            return 0;

        List<string> tempEncounters = new List<string>();

        foreach(string s in _encounters)
        {
            tempEncounters.Add(s);
        }

        if (tempEncounters.Contains(encounterToSpawn))
        {
            count++;
            int index = tempEncounters.IndexOf(encounterToSpawn);
            tempEncounters.RemoveAt(index);
        }
        else if (!tempEncounters.Contains(encounterToSpawn))
        {
            return count;
        }
        return CheckEncounters(tempEncounters, encounterToSpawn, count);
    }

    public void WaveEnded()
    {
        waveActive = false;
        PrepareWave();

        if(currentWave % difficultyRatings[currentDifficulty].increaseFrequency == 0)
        {
            if (difficultyRatings[currentDifficulty].currentEncounterMax < difficultyRatings[currentDifficulty].actualEncounterMax)
                difficultyRatings[currentDifficulty].currentEncounterMax++;
        }

        if(setUpTimer)
            timeToSetUp = StartCoroutine(TimeToSetUp());
    }

    IEnumerator TimeToSetUp()
    {
        for(int i = difficultyRatings[currentDifficulty].timeBetweenWaves; i > 0 ; i--)
        {
            print("Time till next wave is " + i);
            yield return new WaitForSeconds(1);
        }

        StartWave();
    }
}

[System.Serializable]
public class DifficultyRating
{
    public int currentEncounterMax;
    public int actualEncounterMax;
    public int increaseFrequency;

    public int timeBetweenWaves;
    public int carePackageFrequency; 
}
