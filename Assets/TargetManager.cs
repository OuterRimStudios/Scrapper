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

    public Transform GetClosestEnemy(Vector3 searchPos)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in activeEnemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - searchPos;
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
