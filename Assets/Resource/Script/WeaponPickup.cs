using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData; //  WeaponData
    public string pickupMessage = "Press [E] to pick up";
    private PlayerAttack playerAttack;
    public GameObject Sword;
    private bool playerInRange = false;

    private UserInterface userInterface; // showing action text

    private void Start(){
        userInterface = FindFirstObjectByType<UserInterface>();
        playerAttack = FindFirstObjectByType<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (userInterface != null && weaponData != null)
            {
                userInterface.currentActionText.text= $"{pickupMessage} {weaponData.weaponName} (Damage: {weaponData.damage})";
            }
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (userInterface != null)
            {
                userInterface.currentActionText.text ="";
            }

            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerAttack != null && weaponData != null)
            {
                playerAttack.PickupWeapon(weaponData);
                userInterface.currentActionText.text ="";
                Destroy(this.gameObject); // Remove the sword
            }
        }
    }
}
