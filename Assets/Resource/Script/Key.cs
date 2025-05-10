using UnityEngine;

public class Key : MonoBehaviour
{
    public float spinning_speed = 100f;
    public Vector3 rotation = new Vector3(0, 1, 90);

    private AudioManager audioManager;
    private PlayerStat playerStat;
    private UserInterface userInterface;

    private bool playerInRange = false;

    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
        audioManager = FindFirstObjectByType<AudioManager>();
        userInterface = FindFirstObjectByType<UserInterface>();
    }

    void Update()
    {
        transform.Rotate(rotation * spinning_speed * Time.deltaTime); // rotation animation
        if (playerInRange && Input.GetKeyDown(KeyCode.E)){
            PickupKey();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            userInterface.currentActionText.text = "Press E to pick up the key";
        
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            playerInRange = false;
            userInterface.currentActionText.text = "";
        }
    }

    void PickupKey(){
        if (playerStat != null)
        {
            playerStat.keyCount += 1;
            if (audioManager != null){
                audioManager.PlaySFX("KeyPickup");
            }
            userInterface.currentActionText.text = "";
            Destroy(gameObject);
        }
    }
}
