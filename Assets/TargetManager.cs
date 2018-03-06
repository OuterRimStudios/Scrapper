using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> activeEnemies;

    public void Awake()
    {
        GrabEnemies();
    }

    public void GrabEnemies()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
            activeEnemies.Remove(enemyToRemove);
    }

    public List<GameObject> GetAllEnemies()
    {
        return activeEnemies;
    }

    public Transform GetClosestEnemy()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in activeEnemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

}
