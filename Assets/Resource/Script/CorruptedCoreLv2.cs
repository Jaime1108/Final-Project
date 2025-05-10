using UnityEngine;
using TMPro;

public class CorruptedCoreLv2 : MonoBehaviour
{
    public bool isCleanse = false;
    public float cleansingTime = 5f;
    public float resetTime = 6f;
    public int requiredHolyWater = 1;
    public bool ableToExo = false;

    // effect
    public Light pointLight;
    public GameObject CorruptedEffect;

    //audio
    public AudioManager audioManager;
    public AudioSource CursedAudioSource;
    public AudioSource CleansedAudioSource;
    public float gameVolume;
    

    // reference
    public PlayerStat playerStat;
    public UserInterface userInterface;

    private bool isCleansing = false;
    private float cleansingProgress = 0f;
    private float timeOutsideDefense = 0f;

    public bool playerInCleansingRange = false;
    public bool playerInDefenseRange = false;

    void Start()
    {
        isCleanse = false;
        audioManager = FindFirstObjectByType<AudioManager>();
        playerStat = FindFirstObjectByType<PlayerStat>();
        userInterface = FindFirstObjectByType<UserInterface>();
        if(audioManager != null){
            gameVolume = audioManager.masterVolume * audioManager.musicVolume;
        }
        

        //corrupted core initial volume
        CursedAudioSource.loop = true;
        CursedAudioSource.volume = gameVolume;
        CursedAudioSource.Play();

    }

    void Update()
    {   
        if (isCleanse){
        }

        if (playerInCleansingRange && !isCleanse){
            Debug.Log("Player in Cleanse Zone");
            
            if (playerStat.holyWaterCount >= requiredHolyWater){
                ableToExo = true;
                userInterface.currentActionText.text = "Press [F] to begin the cleansing ritual!";
                Debug.Log("Press [F] to begin the cleansing ritual!");
            }
            else{
                if (!isCleanse){
                    userInterface.currentActionText.text = "Not enough Holy Water! Required [" + requiredHolyWater + "] holy water to cleanse";
                }else{
                    ableToExo = false;
                    userInterface.currentActionText.text = "Area is purrified! Corruption has been cleansed";
                }
                
                
            }

            if (Input.GetKeyDown(KeyCode.F) && ableToExo && !isCleansing){
                playerStat.UseHolyWater();
                StartCleansing();
            }
        }else{
            //userInterface.currentActionText.text= "";
        }

        if (isCleansing){
            Cleansing();
        }
        
        
    }


    private void Cleansing()
    {
        if (playerInDefenseRange){
            timeOutsideDefense = 0f;
            cleansingProgress += Time.deltaTime;
            userInterface.currentActionText.text = $"Cleansing the Area... {cleansingProgress:F1}/{cleansingTime}s";

            if (cleansingProgress >= cleansingTime){
                CompleteCleansing();
            }
            Defend();
        }
        else
        {
            timeOutsideDefense += Time.deltaTime;
            if (timeOutsideDefense >= resetTime){
                ResetCleansing();
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
        CursedAudioSource.Stop();
        CleansedAudioSource.loop = true;
        CleansedAudioSource.volume = gameVolume;
        CleansedAudioSource.Play();
        isCleanse = true;
        isCleansing = false;

        if (CorruptedEffect != null){
            CorruptedEffect.SetActive(false);
        }

        if (pointLight != null)
        {
            pointLight.color = new Color(1f, 1f, 0.3f); // bright yellow
        }
        userInterface.currentActionText.text = "Cleanse complete! The corruption has been removed.";
        
    }

    private void ResetCleansing()
    {
        isCleansing = false;
        cleansingProgress = 0f;
        userInterface.currentActionText.text = "Cleansing failed. Stay within the defense zone!";
        
    }

    private void Defend(){
        Debug.Log("Player is within defense range! The core is reacting.");
    }
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            userInterface.currentActionText.text = "";
    }
    }
}