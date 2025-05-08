using UnityEngine;
using UnityEngine.UI;
using TMPro;    
using System;
using UnityEngine.SceneManagement;
public class UserInterface : MonoBehaviour
{
    public PlayerStat PlayerStat;
    public TMP_Text potionCountText;
    public TMP_Text holyWaterCountText;
    public TMP_Text keyCountText;
    //public Text weaponSlotText;
    public Image healthSlider;
    public Image staminaSlider;
    public TMP_Text currentHealth;
    private  PlayerStat playerstat;
    public TMP_Text currentActionText;
    public TMP_Text weaponDamage;
    public AudioManager audioManager;

    // pause game 
    public GameObject pauseMenuUI;
    public GameObject VictoryPanel;
    public GameObject DefeatPanel;
    private bool isPaused = false;
    public CorruptedAltar corruptedAltar;
    void Start(){
        audioManager = FindFirstObjectByType<AudioManager>();
        playerstat = FindFirstObjectByType<PlayerStat>();
        corruptedAltar = FindFirstObjectByType<CorruptedAltar>();
    }

    void Update(){
        currentHealth.text = $"{playerstat.currentHealth}/{playerstat.maxHealth}";
        potionCountText.text = $"Potions: {playerstat.potionCount}";
        holyWaterCountText.text = $"Holy Water: {playerstat.holyWaterCount}";
        keyCountText.text = $"Keys: {playerstat.keyCount}";;

        healthSlider.fillAmount= (float)playerstat.currentHealth/playerstat.maxHealth;
        staminaSlider.fillAmount= (float)playerstat.stamina/playerstat.maxStamina;

        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }
        if(corruptedAltar.isCleanse){
            Invoke("Victory", 10f);
        }
    }

    public void Victory(){
        if (VictoryPanel != null){            
            VictoryPanel.SetActive(true);
            Time.timeScale = 0f; // Freeze game time
        }
    }

    public void Defeat(){
        Invoke("DefeatScreen", 3f);
    }
    public void DefeatScreen(){
        if (DefeatPanel != null){
            audioManager.musicSource.loop = false;
            audioManager.PlayMusic("Defeat");
            DefeatPanel.SetActive(true);
            Time.timeScale = 0f; // Freeze game time
        }
    }

    public void TogglePause(){
        if(isPaused){
            ResumeGame();
        }else{
            PauseGame();
        }
    }

     public void PauseGame(){
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; // reset time scale before loading new scene
        SceneManager.LoadScene("MainMenu"); // replace with your main menu scene name
    }

    public void Restart(){
        Time.timeScale = 1f;  
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
