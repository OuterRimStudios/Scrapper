using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encount : MonoBehaviour
{
    public List<Enemies> enemies;
    List<Transform> availiableSpawnPoints = new List<Transform>();          //A temp list to keep track of the spawnpoints that have not been used

    public void SpawnEcnounters(List<Transform> spawnPoints)
    {
        ResetSpawnPoints(spawnPoints);

        for(int i = 0; i < enemies.Count; i++)                   //Spawn the requested amount of enemies for
        {                                                        //Every enemy in the enemies list
            for(int j = 0; j < enemies[i].amountToSpawn; j++)
            {
                Transform spawnPoint = GetSpawnPoint(spawnPoints);
                GameObject enemy = Instantiate(enemies[i].enemy, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    Transform GetSpawnPoint(List<Transform> spawnPoints)    //Retrieve an empty spawn point
    {
        if (availiableSpawnPoints.Count > 0)
        {
            Transform randomSpawnPoint = availiableSpawnPoints[Random.Range(0, availiableSpawnPoints.Count)];
            availiableSpawnPoints.Remove(randomSpawnPoint);
            return randomSpawnPoint;
        }
        else
        {
            ResetSpawnPoints(spawnPoints);                  //Reset spawns if each point has been used

            Transform randomSpawnPoint = availiableSpawnPoints[Random.Range(0, availiableSpawnPoints.Count)];
            availiableSpawnPoints.Remove(randomSpawnPoint);
            return randomSpawnPoint;
        }
    }

    void ResetSpawnPoints(List<Transform> spawnPoints)     //Resets the availiable spawns after each one has been used
    {                                                      //To allow for a second use of each
        foreach (Transform spawnPoint in spawnPoints)
            availiableSpawnPoints.Add(spawnPoint);
    }
}

[System.Serializable]
public class Enemies
{
    public GameObject enemy;
    public int amountToSpawn;
}
