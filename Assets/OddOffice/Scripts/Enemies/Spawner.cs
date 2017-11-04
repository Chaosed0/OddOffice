using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float minSpawnInterval = 4.0f;
    public float maxSpawnInterval = 10.0f;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(nextSpawnTime);
            GameObject spawned = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, null);
        }
    }
}
