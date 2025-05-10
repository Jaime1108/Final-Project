using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour
{
    public int damage = 10; 
    public AudioManager audioManager;
    
    public AIState AIState;

    void Start(){
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && AIState.State == "Attack")
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null){
                playerStat.TakeDamage(damage);
                audioManager.PlaySFX("Armor Hit");
                Debug.Log("Player took " + damage + " damage from enemy zone.");
            }
        }
    }
}
