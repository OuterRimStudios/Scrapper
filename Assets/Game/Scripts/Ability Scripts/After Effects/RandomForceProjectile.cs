using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RandomForceProjectile : Projectile {

    public float force = 200f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void VisualOnTrigger()
    {
        Vector3 forceDirection = new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1));
        rb.AddForce(forceDirection * force, ForceMode.Force);
    }
}