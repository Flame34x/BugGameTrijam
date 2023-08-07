using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables

    public GameObject objectToSpawn;
    public GameObject slowEnemyToSpawn;
    public GameObject fastEnemyToSpawn;
    public Transform[] enemySpawnPos;
    public float enemySpawnInterval = 2f;
    public float waterSpawnInterval = 2f;
    public float spawnRadius = 1f;
    public Vector2 spawnBoundsMin = new Vector2(-5f, -5f);
    public Vector2 spawnBoundsMax = new Vector2(5f, 5f);
    public int cactiLeft = 3;
    public GameObject[] cacti;

    public GameObject GameOverScreen;

    #endregion

    #region Private Variables

    private float currentScore = 0f;
    private float highScore = 0f;
    private string highScoreKey = "HighScore";
    private PlayerMovement playerMovement;

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        Time.timeScale = 1;
        highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);
        playerMovement = FindObjectOfType<PlayerMovement>();
        WaterSpawning();

        if (playerMovement != null)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void Update()
    {
        currentScore += Time.deltaTime;

        if (cactiLeft <= 0)
        {
            GameOver();
        }
        if (cactiLeft == 2)
        {
            waterSpawnInterval = 1.5f;
            enemySpawnInterval = 2.3f;
            foreach (GameObject cactus in cacti)
            {
                if (cactus != null)
                {
                    cactus.GetComponent<Cactus>().waterLossInterval = 1.2f;
                }
            }
        }
        if (cactiLeft == 1)
        {
            waterSpawnInterval = 1;
            enemySpawnInterval = 2.5f;
            foreach (GameObject cactus in cacti)
            {
                if (cactus != null)
                {
                    cactus.GetComponent<Cactus>().waterLossInterval = 1.5f;
                }
            }
        }

    }

    #endregion

    #region Private Methods

    private void GameOver()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void WaterSpawning()
    {
        StartCoroutine(SpawnWater());
    }

    private IEnumerator SpawnWater()
    {
        while (true)
        {
            // Calculate a random position within the defined bounds
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Spawn the object at the random position
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(waterSpawnInterval);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if the player is not hiding before spawning an enemy
            if (!playerMovement.isHiding)
            {
                SpawnEnemy();
            }

            // Wait for the next spawn interval
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Calculate a random position within the defined bounds
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Spawn the enemy at the random position
        GameObject spawned = Instantiate(slowEnemyToSpawn, getEnemySpawnPos(), Quaternion.identity);
        if (cactiLeft == 3)
        {
            spawned.GetComponent<Enemy>().moveSpeed = 4.2f;
        }
        if (cactiLeft == 2)
        {
            spawned.GetComponent<Enemy>().moveSpeed = 4.5f;
        }
        if (cactiLeft == 1)
        {
            spawned.GetComponent<Enemy>().moveSpeed = 4.7f;
        }
    }

    private Vector3 getEnemySpawnPos()
    {
        Vector3 closestSpawnPos = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Transform spawnPoint in enemySpawnPos)
        {
            // Calculate the distance from the spawn point to the player
            float distanceToPlayer = Vector3.Distance(spawnPoint.position, playerMovement.transform.position);

            // Check if this spawn point is closer to the player than the previous closest
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestSpawnPos = spawnPoint.position;
            }
        }

        return closestSpawnPos;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;

        // Try to find a clear spawn position within the defined bounds
        int maxAttempts = 10; // Maximum number of attempts to find a clear spot
        int attempts = 0;

        do
        {
            float randomX = Random.Range(spawnBoundsMin.x, spawnBoundsMax.x);
            float randomY = Random.Range(spawnBoundsMin.y, spawnBoundsMax.y);
            spawnPosition = new Vector3(randomX, randomY, 0f);

            // Check if there are any colliders at the spawn position
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, spawnRadius);

            // If there are no colliders, break out of the loop and return the spawn position
            if (colliders.Length == 0)
            {
                break;
            }

            attempts++;
        } while (attempts < maxAttempts);

        // If we couldn't find a clear spot within the maximum attempts, just return the last calculated position
        return spawnPosition;
    }

    #endregion

    #region Public Functions

    public void CactusDie()
    {
        cactiLeft -= 1;
    }

    public void PlayerDie()
    {
        GameOver();
    }

    #endregion
    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the scene view to show the spawn radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    #endregion
}
