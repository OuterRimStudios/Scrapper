using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> activeEnemies;
    public List<GameObject> activeFriendlies;

    SpawnManager spawnManager;
    public static TargetManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
    }

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
            {
                activeEnemies.Remove(targetToRemove);
            }
            if (activeEnemies.Count <= 0)
                spawnManager.WaveEnded();
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

    public Transform GetClosestTarget(Vector3 searchPos, string enemyTag, List<string> exclusionTags)
    {
        Transform bestTarget = null;
        if (enemyTag == "Enemy")
        {
            List<GameObject> culledEnemies = Utility.CullList(activeEnemies, exclusionTags);
            foreach (GameObject potentialTarget in culledEnemies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos);
            }
        }
        else if (enemyTag == "Friendly")
        {
            List<GameObject> culledFriendlies = Utility.CullList(activeFriendlies, exclusionTags);
            foreach (GameObject potentialTarget in culledFriendlies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos);
            }
        }

        return bestTarget;
    }

    public Transform GetClosestTargetExcludeSelf(Transform searchPos, string enemyTag)
    {
        Transform bestTarget = null;
        if (enemyTag == "Enemy")
            foreach (GameObject potentialTarget in activeEnemies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos.position);
            }
        else if (enemyTag == "Friendly")
            foreach (GameObject potentialTarget in activeFriendlies)
            {
                bestTarget = CheckDistance(potentialTarget, searchPos.position);
            }

        if (bestTarget == searchPos)
            return null;
        else
            return bestTarget;
    }

    Transform CheckDistance(GameObject potentialTarget, Vector3 searchPos)
    {
        if (!potentialTarget) return null;
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
