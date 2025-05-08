using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorruptedAltar : MonoBehaviour
{
    public bool isCleanse = false;
    public bool ableToExo = false;
    public float cleansingTime = 15f;
    public float resetTime = 6f;

    private float cleansingProgress = 0f;
    private float timeOutsideDefense = 0f;
    public bool isCleansing = false;

    public GameObject CorruptedFlame; 
    public GameObject CleansedFlame;
    public PlayerStat playerstat;
    public UserInterface userinterface;
    public AudioManager audioManager;

    public bool playerInCleansingRange = false;
    public bool playerInDefenseRange = false;
    private Transform player;
    
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        userinterface = FindFirstObjectByType<UserInterface>();
        playerstat = FindFirstObjectByType<PlayerStat>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (CorruptedFlame != null){
            CorruptedFlame.SetActive(true); //flame on
        }
        if (CleansedFlame != null){
            CleansedFlame.SetActive(false); //flame on
        }
    }

    void Update()
    {
        if (player == null || isCleanse) return;

        if (playerInCleansingRange)
        {
            ableToExo = true;
            if (playerstat.holyWaterCount >= 3){
                userinterface.currentActionText.text = "Press [F] to begin the cleansing ritual! Requires 3 Holy Water";}
            else{
                userinterface.currentActionText.text = "Not enough Holy Water! Requires 3 to cleanse.";}
            if (Input.GetKeyDown(KeyCode.F) && playerstat.holyWaterCount >= 3){
                for (int i =0; i < 3; i++){
                    playerstat.UseHolyWater();
                }
                StartCleansing();
            }
        }
        else{
            ableToExo = false;
            userinterface.currentActionText.text = "";
        }

        if (isCleansing)
        {
            if (playerInDefenseRange){
                timeOutsideDefense = 0f;
                cleansingProgress += Time.deltaTime;
                userinterface.currentActionText.text = "Cleansing the Area...";

                if (cleansingProgress >= cleansingTime){
                    CompleteCleansing();
                }
            }
            else{
                timeOutsideDefense += Time.deltaTime;
                if (timeOutsideDefense >= resetTime){
                    ResetCleansing();
                }
            }
        }
    }

    private void StartCleansing()
    {
        if (!isCleansing){
            isCleansing = true;
            cleansingProgress = 0f;
            timeOutsideDefense = 0f;
            Debug.Log("Cleansing started...");
        }
    }

    private void CompleteCleansing(){
        isCleanse = true;
        isCleansing = false;

        if (CorruptedFlame != null)
        {
            CorruptedFlame.SetActive(false); // corrupted flame off
        }
        if (CleansedFlame != null){
            CleansedFlame.SetActive(true); // cleansed flame on
        }
        audioManager.musicSource.loop = true;
        audioManager.PlayMusic("Cleansed");

        userinterface.currentActionText.text = "Cleanse complete! The corruption has been removed. This land is finally free!";
    }

    private void ResetCleansing()
    {
        isCleansing = false;
        cleansingProgress = 0f;
        userinterface.currentActionText.text = "Cleansing failed. You have to stay close to the altar!";
        Debug.Log("Cleansing reset! Player left defense range too long.");
    }

}
