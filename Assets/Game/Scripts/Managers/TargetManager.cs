using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> activeEnemies;
    public List<GameObject> activeFriendlies;

    public void AddTarget(GameObject targetToAdd, string tag)
    {
        if(tag == "Enemy")
            activeEnemies.Add(targetToAdd);
        else if(tag == "Friendly")
            activeFriendlies.Add(targetToAdd);
    }

    public void RemoveTarget(GameObject targetToRemove, string tag)
    {
        if (tag == "Enemy")
        {
            if (activeEnemies.Contains(targetToRemove))
                activeEnemies.Remove(targetToRemove);
        }
        else if(tag == "Friendly")
        {
            if (activeFriendlies.Contains(targetToRemove))
                activeFriendlies.Remove(targetToRemove);
        }
    }

    public List<GameObject> GetAllTargets(string tag)
    {
        if (tag == "Enemy")
            return activeEnemies;
        else if (tag == "Friendly")
            return activeFriendlies;
        else
            return null;
    }

    public Transform GetClosestTarget(Vector3 searchPos, string tag)
    {
        Transform bestTarget = null;
        if (tag == "Enemy")
            foreach (GameObject potentialTarget in activeEnemies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos);
            }
        else if (tag == "Friendly")
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
