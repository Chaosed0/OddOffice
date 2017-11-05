using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float knockbackForce = 50;
    public float knockbackDuration = .1f;

    private Rigidbody rb;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
    }

	public void DoKnockback(Vector3 dir)
    {
        dir.y = 0;

        rb.isKinematic = false;
        rb.AddForce(dir * knockbackForce, ForceMode.Impulse);

        Invoke("StopKnockback", knockbackDuration);
    }

    void StopKnockback()
    {
        // Assumes all enemies are kinematic
        rb.isKinematic = true;
    }
}
