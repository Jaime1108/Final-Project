using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour
{
    public int damage = 10; 
    
    public AIState AIState;

    void Start(){

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && AIState.State == "Attack")
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                playerStat.TakeDamage(damage);
                Debug.Log("Player took " + damage + " damage from enemy zone.");
            }
        }
    }
}
