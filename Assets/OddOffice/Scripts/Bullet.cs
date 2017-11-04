using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 velocity = transform.rotation * Vector3.forward * speed;
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision other)
    {
        rb.useGravity = true;
    }
}
