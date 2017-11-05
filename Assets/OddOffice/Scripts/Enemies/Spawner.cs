using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public bool onlySpawnWhenDead = false;
    public float minSpawnInterval = 4.0f;
    public float maxSpawnInterval = 10.0f;

    private GameObject lastSpawned = null;
    private IEnumerator spawnCoroutine = null;

    void Start()
    {
        if (this.enabled)
        {
            StartSpawning();
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(nextSpawnTime);

            if (!onlySpawnWhenDead || lastSpawned == null)
            {
                GameObject spawned = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, null);
                InitializeEnemy(spawned);
                lastSpawned = spawned;
            }
        }
    }

    void InitializeEnemy(GameObject spawned)
    {
        MugShot mugShot;
        if (mugShot = spawned.GetComponent<MugShot>())
        {
            mugShot.globPool = GameObject.Find("CoffeeProjectilePool").GetComponent<StackPool>();
        }
    }

    void OnEnable()
    {
        StartSpawning();
    }

    void OnDisable()
    {
        StopSpawning();
    }

    void StartSpawning()
    {
        StopSpawning();
        spawnCoroutine = SpawnCoroutine();
        StartCoroutine(spawnCoroutine);
    }

    void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
