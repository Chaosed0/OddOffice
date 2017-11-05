using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    Rigidbody rb;
    ParticleSystem explosion;
    Vector3 initialVelocity;

    public AudioSource explosionAudioPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        explosion = GetComponentInChildren<ParticleSystem>();

        initialVelocity = transform.rotation * Vector3.forward * speed;
        rb.AddForce(initialVelocity, ForceMode.VelocityChange);
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
            Instantiate(explosionAudioPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2);
            Destroy(gameObject);

            Knockback knockback = health.GetComponent<Knockback>();
            if (knockback)
            {
                knockback.DoKnockback(initialVelocity.normalized);
            }
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
