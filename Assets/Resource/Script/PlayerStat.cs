using UnityEngine;
using System.Collections;

public class PlayerStat : MonoBehaviour
{
    public int maxHealth = 100;  // Changed to int
    public int currentHealth;    // Changed to int
    public float healingRate = 1f; // Rate of health recovery per second while using potion.
    public int potionCount = 2;
    public int holyWaterCount = 1;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health.
        Debug.Log("Player start health: " + currentHealth);
    }

    private void Update()
    {
        PassiveHealing();

        // Check if the player is using a potion
        if (Input.GetKey(KeyCode.H) && potionCount > 0) // Suppose H is the key for healing (potion)
        {
            UsePotion(25);
        }
        if(currentHealth == 0){
            Die();
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

            // Gradually heal the player
            while (healAmount > 0 && currentHealth < maxHealth)
            {
                int healingPerFrame = Mathf.FloorToInt(5f * Time.deltaTime);  // Healing per frame is an int

                // Add healing every frame, making sure not to exceed max health
                currentHealth += healingPerFrame;
                healAmount -= healingPerFrame;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }

            Debug.Log($"Potion used. Current Health: {currentHealth}");
        }
    }

    public void UseHolyWater()
    {
        holyWaterCount -= 1;
        Debug.Log($"Holy Water used. Current Holy Water Count: {holyWaterCount}");
    }
    public void Die(){
        //die
    }
}
