using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public List<GameObject> encounters;
    public int maxEncounters;

    public Sprite encounterImage;
    public Sprite[] mechanicImages;

    SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    public void RemoveEncounter(GameObject encounterToRemove)
    {
        if (encounters.Contains(encounterToRemove))
            encounters.Remove(encounterToRemove);

        if (encounters.Count <= 0)
        {
            spawnManager.RemoveEncounter(this);
        }
    }

    private void OnEnable()
    {
        foreach(GameObject go in encounters)
        {
            go.SetActive(true);
        }
    }
}
