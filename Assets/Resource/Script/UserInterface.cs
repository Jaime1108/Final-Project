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
    //public Text weaponSlotText;
    public Image healthSlider;
    public Image staminaSlider;
    public TMP_Text currentHealth;
    private  PlayerStat playerstat;
    public TMP_Text currentActionText;
    public TMP_Text weaponDamage;

    // pause game 
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    void Start(){
        playerstat = FindFirstObjectByType<PlayerStat>();
    }

    void Update(){
        currentHealth.text = $"{playerstat.currentHealth}/{playerstat.maxHealth}";
        potionCountText.text = $"Potions: {playerstat.potionCount}";
        holyWaterCountText.text = $"Holy Water: {playerstat.holyWaterCount}";

        healthSlider.fillAmount= (float)playerstat.currentHealth/playerstat.maxHealth;
        staminaSlider.fillAmount= (float)playerstat.stamina/playerstat.maxStamina;

        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
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
