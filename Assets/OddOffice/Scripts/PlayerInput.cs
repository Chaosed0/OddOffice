using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lookSensitivity = new Vector2(1,1);

    public Camera playerCamera;

    private Movement movement;
    private Gun gun;

    private float cameraRotation = 0;
    
    void Start()
    {
        movement = GetComponent<Movement>();
        gun = GetComponent<Gun>();
    }

	void Update ()
    {
        float forward = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");
        float lookX = Input.GetAxis("Mouse X");
        float lookY = Input.GetAxis("Mouse Y");
        bool fire = Input.GetButtonDown("Fire1");

        cameraRotation = Mathf.Clamp(cameraRotation - lookY, -89, 89);
        playerCamera.transform.localRotation = Quaternion.AngleAxis(cameraRotation, Vector3.right);

        movement.Move(new Vector2(strafe, forward), lookX);

        if (fire)
        {
            gun.Fire();
        }
	}
}
