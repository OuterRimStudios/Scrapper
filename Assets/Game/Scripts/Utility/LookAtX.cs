using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtX : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.root.forward * 100);
    }
}
