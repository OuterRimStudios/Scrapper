using System.Collections;
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
        else if (friendlyTag == "Friendly")
        {
            if (activeFriendlies.Contains(targetToRemove))
                activeFriendlies.Remove(targetToRemove);
        }
    }

    public void RemoveTarget(GameObject targetToRemove, string friendlyTag, Transform followPoint)
    {
        StopTargetting(followPoint);
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

    public void StopTargetting(Transform target)
    {
        if(target)
            target.root.GetComponent<ReferenceManager>().RemoveFollowTarget(target);
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

        return GetFollowPoint(bestTarget);
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

        return GetFollowPoint(bestTarget);
    }

    public Transform GetFollowPoint(Transform target)
    {
        if (target == null) return null;
        Transform followPoint = target.GetComponent<ReferenceManager>().GetFollowPoint();
        return followPoint;
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
            return GetFollowPoint(bestTarget);
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


/*
 * list of target transforms, parallel array of bools to show if position is filled
 * function that returns a transform -> return value is an empty position
 * - cycle through positions or bools to find an empty
 * - mark that position as filled and return it
 * if all positions are filled
 * - instantiate new empty gameobject, take an existing filled position, add multiplier to it to find a further point, add that position to the list, add bool to list
 * 
 */