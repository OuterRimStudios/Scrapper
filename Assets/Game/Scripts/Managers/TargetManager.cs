﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> activeEnemies;
    public List<GameObject> activeFriendlies;

    public void AddTarget(GameObject targetToAdd, string friendlyTag)
    {
        if(friendlyTag == "Enemy")
            activeEnemies.Add(targetToAdd);
        else if(friendlyTag == "Friendly")
            activeFriendlies.Add(targetToAdd);
    }

    public void RemoveTarget(GameObject targetToRemove, string friendlyTag)
    {
        if (friendlyTag == "Enemy")
        {
            if (activeEnemies.Contains(targetToRemove))
                activeEnemies.Remove(targetToRemove);
        }
        else if(friendlyTag == "Friendly")
        {
            if (activeFriendlies.Contains(targetToRemove))
                activeFriendlies.Remove(targetToRemove);
        }
    }

    public List<GameObject> GetAllTargets(string enemyTag)
    {
        if (enemyTag == "Enemy")
            return activeEnemies;
        else if (enemyTag == "Friendly")
            return activeFriendlies;
        else
            return null;
    }

    public Transform GetClosestTarget(Vector3 searchPos, string enemyTag)
    {
        Transform bestTarget = null;
        if (enemyTag == "Enemy")
            foreach (GameObject potentialTarget in activeEnemies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos);
            }
        else if (enemyTag == "Friendly")
            foreach (GameObject potentialTarget in activeFriendlies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos);
            }

        return bestTarget;
    }

    Transform CheckDistance(GameObject potentialTarget, Vector3 searchPos)
    {
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 directionToTarget = potentialTarget.transform.position - searchPos;
        float dSqrToTarget = directionToTarget.sqrMagnitude;
        if (dSqrToTarget < closestDistanceSqr)
        {
            closestDistanceSqr = dSqrToTarget;
            return potentialTarget.transform;
        }
        else
            return null;
    }
}
