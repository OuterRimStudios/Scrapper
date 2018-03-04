using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float baseSpeed;

    float speed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = baseSpeed;
    }

    public void ApplyStun()
    {
        speed = 0;
        print("Stunned " + speed);
    }

    public void RemoveStun()
    {
        speed = baseSpeed;
        print("Stun Removed " + speed);
    }

    public void ApplySlow(float slowAmount)
    {
        print("Slow Amt " + slowAmount);
        float slowPercentage = slowAmount / 100;
        float newSpeed = (baseSpeed * slowPercentage);
        speed -= newSpeed;

        print("AI Slowed -- " + " Slow Percentage : " + slowPercentage + " New Speed : " + newSpeed + " Current Speed: " +  speed);
    }

    public void RemoveSlow()
    {
        speed = baseSpeed;
        print("Slow Removed " + speed);
    }

    public void KnockedBack(float force)
    {
        rb.AddForce((-transform.forward + transform.up) * force, ForceMode.Force);
        print("Knocked Back");
    }
}
