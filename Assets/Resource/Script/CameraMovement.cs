using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // to get player position
    public float cameraSpeed = 5f; // camera following speed for smoothness
    private Vector3 targetPosition; //traveling target
    private bool canMove = false; // to lock camera everytime switching angle
    
    void Update()
    {
        // follow the player smoothly as the player move
        targetPosition = player.position + new Vector3(0,10,-10);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    
        //camera rotation
        if (Input.GetKeyDown(KeyCode.Z) && canMove){
            transform.rotation *= Quaternion.Euler(0, 90, 0);
            canMove = false; //lock rotation
        }
        if (Input.GetKeyDown(KeyCode.X) && canMove){
            transform.rotation *= Quaternion.Euler(0, -90, 0);
            canMove = false; //lock rotation
        }
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X)){
            canMove = true; //unlock rotation after the key are releases
        }
    }
}
