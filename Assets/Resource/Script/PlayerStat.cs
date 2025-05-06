using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    public int maxHealth = 100;  // Changed to int
    public int currentHealth;    // Changed to int
    public float healingRate = 1f; // Rate of health recovery per second while using potion.
    public int potionCount = 2;
    public int holyWaterCount = 1;
    public int keyCount = 1; // Number of keys the player has
    public float maxStamina = 5f;
    public float stamina;

    private UserInterface userinterface;
    private void Start()
    {
        currentHealth = maxHealth; // Initialize health and stamina.
        stamina = maxStamina;
        Debug.Log("Player start health: " + currentHealth);
        userinterface = FindFirstObjectByType<UserInterface>();
    }

    private void Update()
    {
        PassiveHealing();

        // Check if the player is using a potion
        if (Input.GetKey(KeyCode.H) && potionCount > 0) 
        {
            UsePotion(25);
        }
        if(currentHealth == 0){
            userinterface.currentActionText.text = "Our Great Knight has fallen!The Brotherhood shall avenge you!";
            Invoke(nameof(Die),3f);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays within bounds
        Debug.Log("Player current health: " + currentHealth);
        // Add any damage effect like flash or sound here
    }

    void PassiveHealing()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healingRate * Time.deltaTime);  // Use floor to avoid fractional healing
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    public void UsePotion(int healAmount)
    {
        // Only proceed if the player needs healing
        if (currentHealth < maxHealth && potionCount > 0)
        {
            potionCount -= 1;

            
            if (healAmount + currentHealth >= maxHealth){
                currentHealth = maxHealth;
            }
            else{
                currentHealth += healAmount;
            }
        }
    }

    public void UseHolyWater()
    {
        holyWaterCount -= 1;
        Debug.Log($"Holy Water used. Current Holy Water Count: {holyWaterCount}");
    }
    public void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
