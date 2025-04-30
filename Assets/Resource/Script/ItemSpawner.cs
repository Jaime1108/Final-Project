using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject potionPrefab;
    public BoxCollider spawnArea;

    public int minItems = 1;
    public int maxItems = 3;
    public float minDistanceBetweenItems = 1.0f;

    private List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        SpawnPotions();
    }

    private void SpawnPotions()
    {
        if (potionPrefab == null || spawnArea == null) return;

        int spawnCount = Random.Range(minItems, maxItems + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetNonOverlappingPosition();
            Instantiate(potionPrefab, spawnPos, Quaternion.identity, this.transform);

        }
    }

    private Vector3 GetNonOverlappingPosition()
    {
        const int maxAttempts = 20;
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            Vector3 candidate = GetRandomPositionInBox(spawnArea);
            bool tooClose = false;

            foreach (Vector3 used in usedPositions)
            {
                if (Vector3.Distance(candidate, used) < minDistanceBetweenItems)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                usedPositions.Add(candidate);
                return candidate;
            }

            attempt++;
        }

        // If no good spot found, return anyway (fallback)
        return GetRandomPositionInBox(spawnArea);
    }

    public int SpawnPotionsAndReturnCount(){
        int spawnCount = Random.Range(minItems, maxItems + 1);
        for (int i = 0; i < spawnCount; i++){
            Vector3 pos = GetNonOverlappingPosition();
            Instantiate(potionPrefab, pos, Quaternion.identity, this.transform);
        }
        return spawnCount;
    }


    private Vector3 GetRandomPositionInBox(BoxCollider box)
    {
        Bounds bounds = box.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}
