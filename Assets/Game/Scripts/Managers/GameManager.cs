using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static GameManager s_Instance = null;
    public static GameManager instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first GameManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                s_Instance = obj.AddComponent(typeof(GameManager)) as GameManager;
                Debug.Log("Could not locate an GameManager object. GameManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

    public static int currentDifficulty = 0;
    public Transform playerRespawnPoint { get; protected set; }

    public void SetPlayerRespawnPoint(Transform spawnPoint)
    {
        playerRespawnPoint = spawnPoint;
    }
}