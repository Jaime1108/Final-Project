using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 100f;
    public float openDuration = 5f;   // duration in seconds

    public UserInterface userinterface;
    public bool isUnlockable = false;
    public CorruptedCoreLv2 corruptedCore;
    
    public AudioManager audioManager;
    
    private bool playerInRange;
    private bool isOpened = false;

    private Quaternion leftStartRot, rightStartRot;
    private Quaternion leftTargetRot, rightTargetRot;
    private float elapsedTime = 0f;

    private void Start(){
        audioManager = FindFirstObjectByType<AudioManager>();
        leftStartRot = leftDoor.localRotation;
        rightStartRot = rightDoor.localRotation;

        leftTargetRot = leftStartRot*Quaternion.Euler(0f, -openAngle, 0f);
        rightTargetRot = rightStartRot*Quaternion.Euler(0f, openAngle, 0f);

        if(corruptedCore == null){
            isUnlockable = true;
        }
        else{
            isUnlockable = false;
        }

    }

    private void Update()
    {
        if(corruptedCore != null){
            if(corruptedCore.isCleanse){

                Debug.Log("Gate is unlockable");
                isUnlockable = true;
            }
        }

        if (!playerInRange){

        }else{
            if (!isOpened){
                if (isUnlockable){
                    userinterface.currentActionText.text = "Press [F] to open the gate.";
                    if (Input.GetKeyDown(KeyCode.F)){
                        OpenGate();
                    }
                }else
                    {userinterface.currentActionText.text = "The gate is locked! Complete the objective.";
}
                
            }
            else{
                userinterface.currentActionText.text = "Gate is open.";
            }
        }

        if (isOpened && elapsedTime < openDuration){
                AnimateGate();
                userinterface.currentActionText.text = "Gate is opening...";
            }
        
    }

    private void OpenGate()
    {
        isOpened = true;
        elapsedTime = 0f;
        if (audioManager != null){
            audioManager.PlaySFX("Gate Open");
        }
        
    }

    private void AnimateGate()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime/openDuration);

        leftDoor.localRotation = Quaternion.Lerp(leftStartRot, leftTargetRot, t);
        rightDoor.localRotation = Quaternion.Lerp(rightStartRot, rightTargetRot, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            userinterface.currentActionText.text = "";
        }
    }
}
