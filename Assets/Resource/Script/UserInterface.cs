using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public PlayerStat PlayerStat;
    public Text potionCountText;
    public Text holyWaterCountText;
    public Text weaponSlotText;
    public Slider healthSlider;
    public Slider holyWaterSlider;

    public int potionCount = 3; // Example starting potion count.
    public int holyWaterCount = 5; // Example starting holy water count.

    void Start()
    {
        healthSlider.maxValue = PlayerStat.maxHealth;
        holyWaterSlider.maxValue = 10; // Just an example for holy water usage.
    }

    void Update()
    {
        healthSlider.value = PlayerStat.currentHealth;
        potionCountText.text = "Potions: " + potionCount.ToString();
        holyWaterCountText.text = "Holy Water: " + holyWaterCount.ToString();
    }

    public void UsePotion()
    {
        if (potionCount > 0)
        {
            PlayerStat.UsePotion(25); 
            potionCount--;
        }
    }
}
