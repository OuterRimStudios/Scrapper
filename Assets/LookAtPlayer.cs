using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = TargetManager.instance.player.transform;
    }

    private void Update()
    {
        if (player)
            transform.LookAt(player);
    }
}
