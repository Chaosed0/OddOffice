using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    public GameObject[] prefabs;

    void Start()
    {
        GetComponent<Health>().OnDied.AddListener(() => {
            foreach (GameObject go in prefabs)
            {
                Instantiate(go, transform.position, transform.rotation, null);
            }
        });
    }
}
