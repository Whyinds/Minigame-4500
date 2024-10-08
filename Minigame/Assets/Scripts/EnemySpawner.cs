using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float XSpawnRange = 6f;
    public GameObject enemyPrefab;
    public float SpawnRateMin = 0.5f;
    public float SpawnRateMax = 3f;

    float moveRate = 1f;

    bool spawnEnemy = true;
    bool waitingToSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.OnGameOver += StopSpawning;

        if (XSpawnRange < 0) { XSpawnRange *= -1; }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnemy && !waitingToSpawn)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        waitingToSpawn = true;
        yield return new WaitForSeconds(Random.Range(SpawnRateMin, SpawnRateMax));
        waitingToSpawn = false;

        // If game over while waiting
        if (spawnEnemy)
        {
            Vector3 spawnPos = new Vector3(Random.Range(XSpawnRange * -1, XSpawnRange), transform.position.y);
            var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().moveSpeed *= moveRate;
        }
        
    }

    void StopSpawning()
    {
        spawnEnemy = false;
    }

    public void IncreaseSpawnRate(float amount=0.07f)
    {
        SpawnRateMin -= amount;
        SpawnRateMax -= amount*3;

        moveRate += 0.35f;

        if (SpawnRateMin <= 0)
        {
            SpawnRateMin = 0.05f;
        }
        if (SpawnRateMax <= 0)
        {
            SpawnRateMax = SpawnRateMin + 0.5f;
        }
    }
}
