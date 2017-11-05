using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public bool isMonsterDoor;

    public GameObject blackPlane;
    public GameObject doorMesh;
    public Rigidbody colliderContainer;
    public Collider collider;

    private Quaternion openRotation;
    private Quaternion closeRotation;

    private Quaternion colliderOpenRotation;
    private Quaternion colliderCloseRotation;

    void Start()
    {
        if (!isMonsterDoor)
        {
            blackPlane.SetActive(false);
        }
        else
        {
            collider.gameObject.layer = 11;
        }

        closeRotation = doorMesh.transform.localRotation;
        // The Z axis is up for these models
        // WHY I DON'T KNOW JUST GO WITH IT
        openRotation = closeRotation * Quaternion.AngleAxis(-90.0f, new Vector3(0.0f, 0.0f, 1.0f));

        colliderCloseRotation = collider.transform.rotation;
        colliderOpenRotation = colliderCloseRotation * Quaternion.AngleAxis(-90.0f, Vector3.up);
    }

    public void Open()
    {
        doorMesh.transform.localRotation = openRotation;
        if (!isMonsterDoor)
        {
            colliderContainer.MoveRotation(colliderOpenRotation);
        }
    }

    public void Close()
    {
        doorMesh.transform.localRotation = closeRotation;
        if (!isMonsterDoor)
        {
            colliderContainer.MoveRotation(colliderCloseRotation);
        }
    }
}
