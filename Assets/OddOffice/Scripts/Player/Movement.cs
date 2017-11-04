using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveForce;
    public float maxSpeed = 10;

    Vector2 movement;
    float yaw;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate()
    {
        // Rotation
        transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);

        // Speed
        Vector3 desiredPlanarVelocity = transform.forward * movement.y * maxSpeed + transform.right * movement.x * maxSpeed;

        Vector3 planarVelocity = rb.velocity;
        planarVelocity.y = 0.0f;
        planarVelocity = Vector3.ClampMagnitude(planarVelocity, maxSpeed);

        Vector3 relativeForce = (desiredPlanarVelocity - planarVelocity) * moveForce;

        rb.AddForce(relativeForce);
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
