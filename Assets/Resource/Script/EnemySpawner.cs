using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  
    public int numberOfEnemies = 5; 
   
    // Define spawn area manually
    private float minX = -50f, maxX = 50f;
    private float minZ = -50f, maxZ = 50f;
    public float yPosition = 0.5f; // Set this based on your ground's height

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(randomX, yPosition, randomZ);
            
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
