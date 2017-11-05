using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Targeter : MonoBehaviour {

    GameObject player;
    NavMeshAgent thisAgent;

    public float lookRotationSpeed;

	void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thisAgent = GetComponent<NavMeshAgent>();
    }
	
	void Update () {
        thisAgent.destination = player.transform.position;

        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;
        if (lookPos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    public void SetNavigationActive(bool navActive)
    {
        thisAgent.updatePosition = navActive;
    }
}
