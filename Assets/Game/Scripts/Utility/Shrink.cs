using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public Vector3 startingScale;
    public float shrinkSpeed;

    private void OnEnable()
    {
        transform.localScale = startingScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

        if(transform.localScale.magnitude < .1f)
        {
            gameObject.SetActive(false);
        }
    }
}
