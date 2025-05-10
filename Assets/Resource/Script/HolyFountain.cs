using UnityEngine;

public class HolyFountain : MonoBehaviour
{
    public UserInterface userinterface;
    public int holyWaterCount = 2;
    public GameObject holyWater;
    private PlayerStat player;
    private bool playerInRange = false;
    private bool collected = false;

    private void Start(){
        player = FindFirstObjectByType<PlayerStat>();
        userinterface = FindFirstObjectByType<UserInterface>();
    }

    private void Update(){
        if(playerInRange  && !collected){
            userinterface.currentActionText.text = "My faith guides me… [E] to gather the holy water.";
        }
        if (Input.GetKeyDown(KeyCode.E) && playerInRange){
            if(!collected){
                CollectHolyWater();
            }
            else{
                userinterface.currentActionText.text = "The holy water has already been taken."; 
            }
            
        }
    }

    private void CollectHolyWater(){
        collected = true;
        // Add all potions to player inventory
        player.holyWaterCount += holyWaterCount;
        Destroy(holyWater);
    }


    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            userinterface.currentActionText.text = "";
        }
    }
}


