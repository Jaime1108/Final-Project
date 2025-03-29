using UnityEngine;
using System.Collections;
public class PlayerStat : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float healingRate = 5f; // Rate of health recovery per second while using potion.

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health.
    }

    private void Update()
    {
        // Check if the player is using a potion
        if (Input.GetKey(KeyCode.H)) // Suppose H is the key for healing (potion)
        {
            HealOverTime();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        // Add any damage effect like flash or sound here
    }

    void HealOverTime()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healingRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public void UsePotion(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}
