using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeProjectile : MonoBehaviour {

    public ParticleSystem splash;
    public StackPool stackPool;

    private void OnCollisionEnter (Collision collision)
    {
        splash.Play();
        splash.transform.SetParent(null);
        stackPool.Push(gameObject);
    }

    private void OnEnable ()
    {
        GetComponent<BoxCollider>().enabled = false;
        splash.transform.SetParent(transform);
    }
}
