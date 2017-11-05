using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    PlayerInput input;
    Movement movement;

    void Start()
    {
        Health health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        input = GetComponent<PlayerInput>();

        health.OnDied.AddListener(OnDeath);
    }

    void OnDeath()
    {
        rb.freezeRotation = false;
        movement.setCanMove(false);
        input.enabled = false;

        rb.AddForce(Vector3.back * 500.0f);
    }
}
