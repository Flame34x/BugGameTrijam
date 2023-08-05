using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2f;
    public float spawnRadius = 1f;
    public Vector2 spawnBoundsMin = new Vector2(-5f, -5f);
    public Vector2 spawnBoundsMax = new Vector2(5f, 5f);

    private void Start()
    {
        // Start spawning objects at intervals
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Calculate a random position within the defined bounds
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Spawn the object at the random position
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
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

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the scene view to show the spawn radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}