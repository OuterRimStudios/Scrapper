using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Encounter> encounters;
    public static int currentDifficulty = 0;

    public List<DifficultyRating> difficultyRatings;

    int currentWave;

    List<Encounter> activeEncounters = new List<Encounter>();
    bool waveActive;
    bool countingDown;

    private void Start()
    {
        PlayerReady();
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

    public void PlayerReady()
    {
        if (!countingDown)
        {
            countingDown = true;
            StartCoroutine(CountDown());
        }
    }

    public void StartWave()
    {
        waveActive = true;

        for (int i = 0; i < difficultyRatings[currentDifficulty].currentEncounterMax; i++)
        {
            Encounter randomEncounter = encounters[Random.Range(0, encounters.Count)];
            int numberSpawned = 0;
            numberSpawned = CheckEncounters(activeEncounters, randomEncounter, numberSpawned);

            if (numberSpawned < randomEncounter.maxEncounters)
            {
                Encounter newEncounter = Instantiate(randomEncounter);
                activeEncounters.Add(newEncounter);
            }
        }
    }

    int CheckEncounters(List<Encounter> _encounters, Encounter encounterToSpawn, int count)
    {
        if (_encounters.Count <= 0)
            return 0;

        List<Encounter> tempEncounters = _encounters;

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
