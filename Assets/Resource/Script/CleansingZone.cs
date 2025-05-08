using UnityEngine;

public class CleansingZone : MonoBehaviour
{
    public CorruptedAltar altar;
    public CorruptedCore corruptedCore;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInCleansingRange = true;
            }  
        }
    }

    private void OnTriggerExit(Collider other){
        if (altar != null){
            altar.playerInCleansingRange = false;
        }  
    }
}
