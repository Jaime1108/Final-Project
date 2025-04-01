using UnityEngine;
using UnityEngine.UI;
using TMPro;    
using System;

public class UserInterface : MonoBehaviour
{
    public PlayerStat PlayerStat;
    public TMP_Text potionCountText;
    public TMP_Text holyWaterCountText;
    //public Text weaponSlotText;
    public Image healthSlider;
    public TMP_Text currentHealth;
    private  PlayerStat playerstat;
    public TMP_Text currentActionText;

    void Start()
    {
        playerstat = FindObjectOfType<PlayerStat>();
        
        
    }

    void Update()
    {
        currentHealth.text = $"{playerstat.currentHealth}/{playerstat.maxHealth}";
        potionCountText.text = $"Potions: {playerstat.potionCount}";
        holyWaterCountText.text = $"Holy Water: {playerstat.holyWaterCount}";

        healthSlider.fillAmount= (float)playerstat.currentHealth/playerstat.maxHealth;
    }

}
