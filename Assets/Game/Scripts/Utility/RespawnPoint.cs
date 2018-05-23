using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour {

    void OnTriggerExit(Collider other)
    {
        GameManager.instance.SetPlayerRespawnPoint(transform);
    }
}