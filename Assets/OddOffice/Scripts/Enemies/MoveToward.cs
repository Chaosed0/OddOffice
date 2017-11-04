using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class MoveToward : MonoBehaviour
{
    public GameObject target;
    public bool flying = false;
    private Movement movement;

    void Awake()
    {
        movement = GetComponent<Movement>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        if (target == null)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        movement.Move(new Vector2(0.0f, 1.0f));
    }
}