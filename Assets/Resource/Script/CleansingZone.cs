using UnityEngine;

public class CleansingZone : MonoBehaviour
{
    public CorruptedAltar altar;
    public CorruptedCoreLv2 corruptedCore;

    

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInCleansingRange = true;
            }else{
                corruptedCore.playerInCleansingRange = true;
            }
            //Debug.Log("Player In cleanse zone");
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInCleansingRange = false;
            }else{
                corruptedCore.playerInCleansingRange = false;
            }
            //Debug.Log("Player go out of cleanse zone");
        } 
    }
}
