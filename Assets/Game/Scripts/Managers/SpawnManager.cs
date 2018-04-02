using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
	public List<Encounter> encounters;
	public static int currentDifficulty = 0;
	public List<Transform> spawnPositions;
	List<Transform> availableSpawnPositions = new List<Transform> ();

	public List<DifficultyRating> difficultyRatings;

    public Text currentWaveText;
    public Text timeToNextWaveText;

	public int currentWave;

	List<Encounter> nextEncounters = new List<Encounter> ();
	List<string> nextEncounterObjects = new List<string> ();
	bool waveActive;
	bool countingDown;

	bool setUpTimer;

	Coroutine timeToSetUp;

	private void Start ()
	{
		ResetSpawnpoints ();
		PrepareWave ();
		PlayerReady ();

		//   if (currentDifficulty == 1 || currentDifficulty == 2)
		setUpTimer = true;

        currentWaveText.text = "Wave: " + 1;
	}

	public void PlayerReady ()
	{
		if (!countingDown) {
			countingDown = true;
			StartCoroutine (CountDown ());
		}
	}

	IEnumerator CountDown ()
	{
		for (int i = 3; i > 0; i--) {
            timeToNextWaveText.text = "Wave Starts in: " + i;
            yield return new WaitForSeconds (1);
		}
        timeToNextWaveText.text = "";
        countingDown = false;
		StartWave ();
	}

	void PrepareWave ()
	{
		for (int i = 0; i < difficultyRatings [currentDifficulty].currentEncounterMax; i++) {
			int encounterIndex = Random.Range (0, encounters.Count - 1);
			Encounter randomEncounter = encounters [encounterIndex];

			int numberSpawned = 0;
			numberSpawned = CheckEncounters (nextEncounterObjects, randomEncounter.name, numberSpawned);

			if (numberSpawned < randomEncounter.maxEncounters) {
				Transform spawnLocation = GetSpawnPosition ();
				Encounter newEncounter = Instantiate (randomEncounter, spawnLocation.position, spawnLocation.rotation);

				newEncounter.name = randomEncounter.name;

				nextEncounters.Add (newEncounter);
				nextEncounterObjects.Add (newEncounter.name);
			}
		}

		foreach (Encounter encounter in nextEncounters) {
			encounter.gameObject.SetActive (false);
		}
	}

	Transform GetSpawnPosition ()
	{
		int randomLocation = Random.Range (0, availableSpawnPositions.Count - 1);
		Transform spawnPoint = availableSpawnPositions [randomLocation];
		availableSpawnPositions.Remove (spawnPoint);
		return spawnPoint;
	}

	public void RemoveEncounter (Encounter _encounter)
	{
		if (nextEncounters.Contains (_encounter)) {
			nextEncounters.Remove (_encounter);
			nextEncounterObjects.Remove (_encounter.name);
			Destroy (_encounter.gameObject);
		}
	}

	public void StartWave ()
	{
		waveActive = true;
		currentWave++;
        currentWaveText.text = "Wave: " + currentWave;

		if (timeToSetUp != null)
			StopCoroutine (timeToSetUp);

		foreach (Encounter encounter in nextEncounters) {
			encounter.gameObject.SetActive (true);
		}
	}

	int CheckEncounters (List<string> _encounters, string encounterToSpawn, int count)
	{
		if (nextEncounterObjects.Count <= 0)
			return 0;

		List<string> tempEncounters = new List<string> ();

		foreach (string s in _encounters) {
			tempEncounters.Add (s);
		}

		if (tempEncounters.Contains (encounterToSpawn)) {
			count++;
			int index = tempEncounters.IndexOf (encounterToSpawn);
			tempEncounters.RemoveAt (index);
		} else if (!tempEncounters.Contains (encounterToSpawn)) {
			return count;
		}
		return CheckEncounters (tempEncounters, encounterToSpawn, count);
	}

	public void WaveEnded ()
	{
		waveActive = false;
		ResetSpawnpoints ();
		PrepareWave ();

		if (currentWave % difficultyRatings [currentDifficulty].increaseFrequency == 0) {
			if (difficultyRatings [currentDifficulty].currentEncounterMax < difficultyRatings [currentDifficulty].actualEncounterMax)
				difficultyRatings [currentDifficulty].currentEncounterMax++;
		}

		if (setUpTimer)
			timeToSetUp = StartCoroutine (TimeToSetUp ());
	}

	void ResetSpawnpoints ()
	{
		availableSpawnPositions.Clear ();
		foreach (Transform point in spawnPositions) {
			availableSpawnPositions.Add (point);
		}
	}

	IEnumerator TimeToSetUp ()
	{
		for (int i = difficultyRatings [currentDifficulty].timeBetweenWaves; i > 0; i--) {
            timeToNextWaveText.text = "Next Wave in: " + i;
			yield return new WaitForSeconds (1);
		}

		StartWave ();
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
