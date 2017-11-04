using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    Rigidbody rb;
    ParticleSystem explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        explosion = GetComponentInChildren<ParticleSystem>();

        Vector3 velocity = transform.rotation * Vector3.forward * speed;
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision other)
    {
        Health health = other.gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            health.DealDamage(10);
            explosion.transform.SetParent(null);
            explosion.transform.position = other.contacts[0].point + (transform.position - other.transform.position).normalized * .25f;
            explosion.Play();
            Destroy(explosion, 2);
            Destroy(gameObject);
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
