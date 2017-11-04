using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveForce;
    public float maxSpeed = 10;
    public float jumpForce = 100;

    Vector2 movement;
    private bool jump = false;
    private bool isGrounded = false;
    private bool canMove = true;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate()
    {
        Vector2 movement = this.movement;
        if (!canMove)
        {
            movement = Vector2.zero;
        }

        Vector3 desiredPlanarVelocity = transform.forward * movement.y * maxSpeed + transform.right * movement.x * maxSpeed;

        Vector3 planarVelocity = rb.velocity;
        planarVelocity.y = 0.0f;
        planarVelocity = Vector3.ClampMagnitude(planarVelocity, maxSpeed);

        Vector3 relativeForce = (desiredPlanarVelocity - planarVelocity) * moveForce;

        rb.AddForce(relativeForce);

        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1.1f);

        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jump = false;
    }

    public void Move(Vector2 movement)
    {
        this.movement = movement;
        if (this.movement.magnitude > 1)
        {
            this.movement = this.movement.normalized;
        }
    }

    public void AddToYaw(float yaw)
    {
        transform.rotation *= Quaternion.AngleAxis(yaw, Vector3.up);
    }

    public void Jump()
    {
        jump = true;
    }

    public void setCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
