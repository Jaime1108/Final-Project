using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject position1;
    public GameObject position2;
    public GameObject position3;

    public GameObject Item;
    
    public int numberOfItemSpawning = 0;
    private List<Transform> spawnPositions = new List<Transform>();


    private void Start(){
        // Add positions to list
        spawnPositions.Add(position1.transform);
        spawnPositions.Add(position2.transform);
        spawnPositions.Add(position3.transform);
        // get random number from 1 to 3
        numberOfItemSpawning = Random.Range(1, 4); // Range is [1,4), so 1–3 inclusive
        // Spawn the items
        SpawnItems(numberOfItemSpawning);
    
    }

    private void SpawnItems(int numberOfItemSpawning){
        for (int i = 0; i < numberOfItemSpawning; i++){
            GameObject potion = Instantiate(Item, spawnPositions[i].position, Quaternion.identity);
            potion.transform.SetParent(spawnPositions[i]);
        }
    }

    
}
