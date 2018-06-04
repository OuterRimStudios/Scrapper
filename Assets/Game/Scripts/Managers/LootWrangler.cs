using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWrangler : MonoBehaviour {

    #region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static LootWrangler s_Instance = null;
    public static LootWrangler instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first LootWrangler object in the scene.
                s_Instance = FindObjectOfType(typeof(LootWrangler)) as LootWrangler;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("LootWrangler");
                s_Instance = obj.AddComponent(typeof(LootWrangler)) as LootWrangler;
                Debug.Log("Could not locate an LootWrangler object. LootWrangler was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

    public List<AbilityItem> abilityItems;

    public AbilityItem GetRandomAbility()
    {
        return abilityItems[Random.Range(0, abilityItems.Count)];
    }
}