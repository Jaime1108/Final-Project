using UnityEngine;

public class DefenseZone : MonoBehaviour
{
    public CorruptedAltar altar;
    public CorruptedCore corruptedCore;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInDefenseRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInDefenseRange = true;
            }
        }
    }
}
