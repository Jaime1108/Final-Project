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
    public GameObject instructionPanel;
    public CharacterCustomization CharacterCustomization;
    public Toggle capeToggle;


    // resolution
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] ResolutionList;
    private List<Resolution> uniqueResolutionList = new List<Resolution>();
    private List<string> resolutionStringList = new List<string>();
    private int currentResolutionPosition = 0;

    // sound manager 
    public Scrollbar masterScrollbar;
    public Scrollbar musicScrollbar;
    public Scrollbar sfxScrollbar;


    void Start()
    {
        difficultyPanel.SetActive(false);
        customizationPanel.SetActive(false);
        optionsPanel.SetActive(false);
        instructionPanel.SetActive(false);

        //Sound manager
        masterScrollbar.value = AudioManager.Instance.masterVolume;
        musicScrollbar.value = AudioManager.Instance.musicVolume;
        sfxScrollbar.value = AudioManager.Instance.sfxVolume;

        masterScrollbar.onValueChanged.AddListener(SetMasterVolume);
        musicScrollbar.onValueChanged.AddListener(SetMusicVolume);
        sfxScrollbar.onValueChanged.AddListener(SetSFXVolume);

        //resolution
        ResolutionList = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        string resolution;
        for (int i = 0; i < ResolutionList.Length; i++)
        {
            resolution = ResolutionList[i].width + " x " + ResolutionList[i].height;

            if (!resolutionStringList.Contains(resolution))
            {
                resolutionStringList.Add(resolution);
                uniqueResolutionList.Add(ResolutionList[i]);

                if (ResolutionList[i].width == Screen.currentResolution.width && ResolutionList[i].height == Screen.currentResolution.height){
                    currentResolutionPosition = uniqueResolutionList.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(resolutionStringList);
        resolutionDropdown.value = currentResolutionPosition;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }



    // Main menu buttons
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //option menu
    public void OpenOption()
    {
        optionsPanel.SetActive(true);
    }
    public void CloseOption(){
        optionsPanel.SetActive(false);
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

    //instrution
    public void OpenInstruction(){
        instructionPanel.SetActive(true);
    }
    public void CloseInstruction(){
        instructionPanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
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

    
}
