using UnityEngine;

public class DefenseZone : MonoBehaviour
{
    public CorruptedAltar altar;
    public CorruptedCoreLv2 corruptedCore;


    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInDefenseRange = true;
            }else{
                corruptedCore.playerInDefenseRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            if (altar != null){
                altar.playerInDefenseRange = false;
            }else{
                corruptedCore.playerInDefenseRange = false;
            }
        }
    }
}
