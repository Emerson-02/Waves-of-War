using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject shooterPrefab; 
    public GameObject chaserPrefab;
    public GameObject player; 
    public int spawnInterval = 2; 
    public float spawnDistance = 10f; 
    void Start()
    {
        spawnInterval = UIController.spawnValue;

        if (spawnInterval < 1)
        {
            spawnInterval = 2;
        }

        StartCoroutine(SpawnEnemy());

    }

    IEnumerator SpawnEnemy()
    {
        if(player != null)
        {
            while (true)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnDistance;
                Vector3 spawnPosition = new Vector3(player.transform.position.x + randomDirection.x, player.transform.position.y + randomDirection.y, 0);

                if (Physics2D.OverlapCircle(spawnPosition, 1f) == null)
                {
                    GameObject enemyToSpawn;
                    float randomValue = Random.value;
                    if (randomValue < 0.6f)
                    {
                        enemyToSpawn = shooterPrefab;
                    }
                    else
                    {
                        enemyToSpawn = chaserPrefab;
                    }

                    Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
