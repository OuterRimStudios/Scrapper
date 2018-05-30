using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Friendly")
            GameManager.instance.SetPlayerRespawnPoint(transform);
    }
}