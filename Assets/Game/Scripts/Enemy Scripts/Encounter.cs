using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public List<GameObject> encounters;
    public int maxEncounters;

    public delegate void EncounterEvents(Encounter encounter);
    public event EncounterEvents OnRemoveEncounter;

    public void RemoveEncounter(GameObject encounterToRemove)
    {
        if (encounters.Contains(encounterToRemove))
            encounters.Remove(encounterToRemove);

        if (encounters.Count <= 0)
        {
            OnRemoveEncounter(this); //<-- Null
            Destroy(gameObject);
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
