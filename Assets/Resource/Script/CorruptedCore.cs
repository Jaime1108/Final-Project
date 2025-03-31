using UnityEngine;

public class CorruptedCore : MonoBehaviour
{
    public bool isCleanse = false;
    public bool ableToExo = false;

    public float cleansingRange = 4f;
    public float defenseRange = 10f;
    public float cleansingTime = 5f;
    public float resetTime = 6f; 
    private Transform player;
    private bool isCleansing = false;
    private float cleansingProgress = 0f;
    private float timeOutsideDefense = 0f;
    public Light pointLight;

    void Start()
    {
        // Find the player GameObject by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null){
            player = playerObj.transform;
        }
        else{
            Debug.LogWarning("Player not found! Make sure the Player GameObject has the tag 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if( cleansingProgress >= cleansingTime && isCleansing){
                isCleanse = true;
                
                Debug.Log("✨ Cleanse complete! The corruption has been removed. ✨");
                pointLight.color = new Color(1f, 1f, 0.3f); // RGB values for bright yellow
            
                isCleansing = false;
                return;
            }
            if (distance <= cleansingRange && !isCleanse){
                ableToExo = true;
                Debug.Log("Press 'F' to begin the cleansing ritual!");
                if(Input.GetKeyDown(KeyCode.F)){
                    StartCleansing();
                }
        
            }
            else{
                ableToExo = false;}

            if (isCleansing && distance <= defenseRange){
                timeOutsideDefense = 0f;
                cleansingProgress += Time.deltaTime;
                Debug.Log($"Cleansing progress: {cleansingProgress}/{cleansingTime}");
                Defend();
            }
            else if (isCleansing){
                timeOutsideDefense  += Time.deltaTime;
                if (timeOutsideDefense >= resetTime)
                {
                    cleansingProgress = 0f;
                    isCleansing = false;
                    Debug.Log("Cleansing reset! Player left defense range too long.");
                }
            }
        }
    }

    void StartCleansing(){
        if (!isCleansing){
                isCleansing = true;
                Debug.Log("Cleansing started...");
            }
            timeOutsideDefense = 0f;
    }

    void Defend()
    {
        // This function can trigger enemies or other defenses
        Debug.Log("Player is within defense range! The core is reacting.");
    }
}
