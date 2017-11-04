using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveForce;

    Vector2 movement;
    float yaw;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate()
    {
        transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
        rb.AddForce(transform.forward * movement.y * moveForce +  transform.right * movement.x * moveForce);
	}

    public void Move(Vector2 movement, float yaw)
    {
        this.movement = movement;
        if (this.movement.magnitude > 1)
        {
            this.movement = this.movement.normalized;
        }
        this.yaw += yaw;
    }
}
