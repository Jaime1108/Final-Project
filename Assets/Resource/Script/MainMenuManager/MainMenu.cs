using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject customizationPanel;
    public GameObject optionsPanel;
    public CharacterCustomization CharacterCustomization;
    public Toggle capeToggle;


    // resolution
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] allResolutions;
    private List<Resolution> uniqueResolutionList = new List<Resolution>();
    private List<string> resolutionStringList = new List<string>();
    private int currentResolutionIndex = 0;

    // sound manager 
    public Scrollbar masterScrollbar;
    public Scrollbar musicScrollbar;
    public Scrollbar sfxScrollbar;


    void Start()
    {
        difficultyPanel.SetActive(false);
        customizationPanel.SetActive(false);
        optionsPanel.SetActive(false);

        //Sound manager
        masterScrollbar.value = AudioManager.Instance.masterVolume;
        musicScrollbar.value = AudioManager.Instance.musicVolume;
        sfxScrollbar.value = AudioManager.Instance.sfxVolume;

        masterScrollbar.onValueChanged.AddListener(SetMasterVolume);
        musicScrollbar.onValueChanged.AddListener(SetMusicVolume);
        sfxScrollbar.onValueChanged.AddListener(SetSFXVolume);

        //resolution
        allResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        string newRes;
        for (int i = 0; i < allResolutions.Length; i++)
        {
            newRes = allResolutions[i].width + " x " + allResolutions[i].height;

            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                uniqueResolutionList.Add(allResolutions[i]);

                if (allResolutions[i].width == Screen.currentResolution.width &&
                    allResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutionList.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(resolutionStringList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }



    // Main menu buttons
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //option menu
    public void OnOptionsPressed()
    {
        optionsPanel.SetActive(true);
    }



    public void ChangeResolution(int index)
    {
        Resolution res = uniqueResolutionList[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        Debug.Log("Resolution set to: " + res.width + " x " + res.height);
        Debug.Log("Screen resolution after change: " + Screen.width + " x " + Screen.height);

    }
    public void SetMasterVolume(float value){
        AudioManager.Instance.masterVolume = value;
    }

    public void SetMusicVolume(float value){
        AudioManager.Instance.musicVolume = value;
    }

    public void SetSFXVolume(float value){
        AudioManager.Instance.sfxVolume = value;
    }




    public void OnQuitPressed()
    {
        Application.Quit();
    }

    // Difficulty buttons
    public void SelectEasy()
    {
        PlayerPrefs.SetString("Difficulty", "Easy");
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMedium()
    {
        PlayerPrefs.SetString("Difficulty", "Normal");
        SceneManager.LoadScene("GameScene");
    }

    public void SelectHard()
    {
        PlayerPrefs.SetString("Difficulty", "Hard");
        SceneManager.LoadScene("GameScene");
    }

    public void CloseDifficulty()
    {
        difficultyPanel.SetActive(false);
    }

    // Customization buttons
    public void Red(){
        CharacterCustomization.ChangeArmorColor(Color.red);
    }

    public void White(){
        CharacterCustomization.ChangeArmorColor(Color.white);
    }

    public void Yellow(){
        CharacterCustomization.ChangeArmorColor(Color.yellow);
    }

    public void Black(){
        CharacterCustomization.ChangeArmorColor(Color.black);
    }

    public void ToggleCape(){
        {
            bool finalState = capeToggle.isOn;  // Directly read from Toggle
            CharacterCustomization.ToggleCape(finalState);
        }
    }

    public void OpenCustomization(){
        customizationPanel.SetActive(true);
    }
    public void CloseCustomization(){
        customizationPanel.SetActive(false);
    }

    // Options buttons
    public void OnCloseOptions()
    {
        optionsPanel.SetActive(false);
    }
}
