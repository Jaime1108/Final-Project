using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Chest : MonoBehaviour
{
    public Transform chestLid;          // The lid to animate
    public float openAngle = 100f;
    public float openDuration = 2f;
    public UserInterface userinterface;
    public ItemSpawner itemSpawner;     // Reference to the item spawner inside chest
    public GameObject itemSpawnArea;
    private bool inRange = false;
    private bool isOpened = false;
    private bool itemsCollected = false;
    private int itemCount = 0;
    private PlayerStat player;
    public AudioManager audioManager;
    


    private Quaternion hingeStartRot;
    private Quaternion hingeTargetRot;
    private float elapsedTime = 0f;


    private void Start(){
        player = FindFirstObjectByType<PlayerStat>();
        audioManager = FindFirstObjectByType<AudioManager>();
        hingeStartRot = chestLid.localRotation;
        hingeTargetRot = hingeStartRot * Quaternion.Euler(openAngle, 0f, 0f);
    }

    private void Update(){
        if (inRange){
            if (!isOpened){
                if (player.keyCount > 0){
                    userinterface.currentActionText.text = "Press [F] to unlock";
                    if (Input.GetKeyDown(KeyCode.F)){
                        player.keyCount--;
                        OpenChest();
                    }
                }else{userinterface.currentActionText.text =  "You need a key to unlock this chest!";}
            }else if (!itemsCollected){
                userinterface.currentActionText.text = $"Press [E] to collect {itemCount} Potion";
                if (Input.GetKeyDown(KeyCode.E)){
                    CollectItems(player);}
                }else{userinterface.currentActionText.text = "Chest is empty.";}

            
        }
        if (isOpened && elapsedTime < openDuration){
            AnimateChestOpen();
        }

    }

    private void OpenChest(){
        
        isOpened = true;
        elapsedTime = 0f;
        if (itemSpawner != null){
            itemCount = itemSpawner.numberOfItemSpawning;
        }
    }
    private void AnimateChestOpen(){
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime/openDuration);
        chestLid.localRotation = Quaternion.Lerp(hingeStartRot, hingeTargetRot, t);

    }

    private void CollectItems(PlayerStat player){
        itemsCollected = true;
        // Add all potions to player inventory
        player.potionCount += itemCount;
        userinterface.currentActionText.text = $"Collected {itemCount} potion(s)";
        Destroy(itemSpawnArea);
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
