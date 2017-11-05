using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeProjectile : MonoBehaviour {

    public float damage = 5;
    public ParticleSystem splash;
    public StackPool stackPool;
    public float grav;

    public AudioSource splashAudioPrefab;

    private Rigidbody rb;
    private BoxCollider bc;

    private void OnCollisionEnter (Collision collision)
    {
        splash.Play();
        splash.transform.SetParent(null);

        if (collision.collider.gameObject.layer == 9)
        {
            collision.collider.GetComponent<Health>().DealDamage(damage);
            Instantiate(splashAudioPrefab, this.transform.position, Quaternion.identity);
        }

        stackPool.Push(gameObject);
    }

    private void OnEnable ()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        bc.enabled = false;
        Invoke("EnableCollider", .5f);
        splash.transform.SetParent(transform);
    }

    void Update()
    {
        rb.velocity += Vector3.up * grav * Time.deltaTime;
    }

    void EnableCollider()
    {
        bc.enabled = true;
    }
}
