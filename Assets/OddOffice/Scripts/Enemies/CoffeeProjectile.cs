using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeProjectile : MonoBehaviour {

    public ParticleSystem splash;
    public StackPool stackPool;
    public float grav;

    private Rigidbody rb;
    private BoxCollider bc;

    private void OnCollisionEnter (Collision collision)
    {
        splash.Play();
        splash.transform.SetParent(null);
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
