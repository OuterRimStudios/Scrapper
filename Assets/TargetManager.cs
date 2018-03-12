using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> activeEnemies;
    public List<GameObject> activeFriendlies;

    public void Awake()
    {
        GrabEnemies();
        GrabFriendlies();
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

    public void GrabFriendlies()
    {
        activeFriendlies = GameObject.FindGameObjectsWithTag("Friendly").ToList();
    }

    public void RemoveFriendly(GameObject friendlyToRemove)
    {
        if (activeFriendlies.Contains(friendlyToRemove))
            activeFriendlies.Remove(friendlyToRemove);
    }

    public List<GameObject> GetAllFriendlies()
    {
        return activeFriendlies;
    }

    public Transform GetClosestFriendly(Vector3 searchPos)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in activeFriendlies)
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
