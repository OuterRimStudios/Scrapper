using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    Camera cam;

	void Start ()
    {
        cam = Camera.main;	
	}
	
	void Update ()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        transform.LookAt(ray.GetPoint(10));
	}
}
