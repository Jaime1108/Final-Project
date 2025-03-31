using UnityEngine;
using System.Collections;
public class PlayerStat : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float healingRate = 1f; // Rate of health recovery per second while using potion.
    public int potionCount = 2;
    public int holyWaterCount = 1;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health.
    }

    private void Update()
    {
        PassiveHealing();
        // Check if the player is using a potion
        if (Input.GetKey(KeyCode.H) && potionCount > 0) // Suppose H is the key for healing (potion)
        {
            UsePotion(25f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        // Add any damage effect like flash or sound here
    }

    void PassiveHealing()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healingRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public void UsePotion(float healAmount){
    // Only proceed if the player needs healing
    if (currentHealth < maxHealth && potionCount > 0)
    {
        potionCount -= 1; 
        
        
        float healingPerFrame = 5f * Time.deltaTime;  

        // Gradually heal the player
        while (healAmount > 0 && currentHealth < maxHealth)
        {
            // Add healing every frame, making sure not to exceed max health
            currentHealth += healingPerFrame;
            healAmount -= healingPerFrame;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        Debug.Log($"Potion used. Current Health: {currentHealth}");
    }
    public void UseHolyWater(float healAmount){
        holyWaterCount -=1;
    }
}
}
