using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;
    public BoxCollider spawnArea; // Assign this in the Inspector
    public float yPosition = 0.5f; // Optional: fixed height

    void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("Spawn area not assigned.");
            return;
        }

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        Bounds bounds = spawnArea.bounds;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 spawnPosition = new Vector3(randomX, yPosition, randomZ);

            // Randomize rotation only on Y
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            Instantiate(enemyPrefab, spawnPosition, randomRotation);
        }
    }
}
