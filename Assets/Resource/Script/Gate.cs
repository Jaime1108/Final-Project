using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 100f;
    public float openDuration = 2f;   // Duration in seconds

    public UserInterface userinterface;
    public bool isUnlockable = false;

    private bool playerInRange;
    private bool isOpened = false;

    private Quaternion leftStartRot, rightStartRot;
    private Quaternion leftTargetRot, rightTargetRot;
    private float elapsedTime = 0f;

    private void Start()
    {
        leftStartRot = leftDoor.localRotation;
        rightStartRot = rightDoor.localRotation;

        leftTargetRot = leftStartRot * Quaternion.Euler(0f, -openAngle, 0f);
        rightTargetRot = rightStartRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (!isOpened)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isUnlockable)
                {
                    OpenGate();
                }
                else
                {
                    userinterface.currentActionText.text = "The gate is locked! Complete the objective.";
                }
            }
        }
        else if (elapsedTime < openDuration)
        {
            AnimateGate();
            userinterface.currentActionText.text = "Gate is opening...";
        }
        else
        {
            userinterface.currentActionText.text = "Gate is open.";
        }
    }

    private void OpenGate()
    {
        isOpened = true;
        elapsedTime = 0f;
    }

    private void AnimateGate()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / openDuration);

        leftDoor.localRotation = Quaternion.Lerp(leftStartRot, leftTargetRot, t);
        rightDoor.localRotation = Quaternion.Lerp(rightStartRot, rightTargetRot, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (isOpened)
            {
                userinterface.currentActionText.text = "Gate is open.";
            }
            else if (isUnlockable)
            {
                userinterface.currentActionText.text = "Press [F] to open the gate.";
            }
            else
            {
                userinterface.currentActionText.text = "Complete the objective to open the gate.";
            }
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
