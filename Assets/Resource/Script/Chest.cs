using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Chest : MonoBehaviour
{
    public Transform chestLid;          // The lid to animate
    public float openAngle = 120f;
    public float openSpeed = 90f;
    public UserInterface userinterface;
    public ItemSpawner itemSpawner;     // Reference to the item spawner inside chest
    public GameObject itemSpawnArea;
    private bool inRange = false;
    private bool isOpened = false;
    private bool itemsCollected = false;
    private int itemCount = 0;
    private PlayerStat player;

    private void Start(){
        player = FindFirstObjectByType<PlayerStat>();
    }

    private void Update(){
        if (inRange){
            if (!isOpened){
                userinterface.currentActionText.text = "Press [F] to unlock";
                if (Input.GetKeyDown(KeyCode.F)){
                    if (player.keyCount > 0){
                        player.keyCount--;
                        OpenChest();}
                    else{
                        userinterface.currentActionText.text =  "You need a key to unlock this chest!";
                    }
                }
            }
            else if (!itemsCollected){
                userinterface.currentActionText.text = $"Press [E] to collect {itemCount} item(s)";
                if (Input.GetKeyDown(KeyCode.E)){
                    CollectItems(player);}
            }
            else{
                userinterface.currentActionText.text = "Chest is empty.";}
        }
    }

    private void OpenChest(){
        isOpened = true;
        StartCoroutine(RotateLid());
        if (itemSpawner != null){
            itemCount = itemSpawner.numberOfItemSpawning;
        }
    }

    private void CollectItems(PlayerStat player){
        itemsCollected = true;
        // Add all potions to player inventory
        player.potionCount += itemCount;
        userinterface.currentActionText.text = $"Collected {itemCount} potion(s)";
        Destroy(itemSpawnArea);
    }


    private IEnumerator RotateLid(){
        float currentAngle = 0f;
        while (currentAngle < openAngle)
        {
            float step = openSpeed * Time.deltaTime;
            chestLid.Rotate(Vector3.right, step);
            currentAngle += step;
            yield return null;
        }

        // Snap to exact angle
        Vector3 euler = chestLid.localEulerAngles;
        euler.x = openAngle;
        chestLid.localEulerAngles = euler;
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            inRange = false;
            userinterface.currentActionText.text = "";
        }
    }
}
