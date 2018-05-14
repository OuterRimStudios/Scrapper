using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public GameObject trainingDummy;
    public List<Transform> spawnpoints;

    List<GameObject> spawnObjects = new List<GameObject>();

    bool checkDummy;

    private void Start()
    {
        for(int i = 0; i < spawnpoints.Count; i++)
        {
            GameObject dummy = Instantiate(trainingDummy, spawnpoints[i].transform.position, spawnpoints[i].transform.rotation);
            spawnObjects.Add(dummy);
        }
    }

    private void Update()
    {
        if(!checkDummy)
        {
            checkDummy = true;

            for(int i = 0; i < spawnObjects.Count; i++)
            {
                if(spawnObjects[i] == null)
                {
                    GameObject dummy = Instantiate(trainingDummy, spawnpoints[i].transform.position, spawnpoints[i].transform.rotation);
                    spawnObjects[i] = dummy;
                }
            }
            StartCoroutine(CheckDummy());
        }
    }

    IEnumerator CheckDummy()
    {
        yield return new WaitForSeconds(5);
        checkDummy = false;
    }
}
