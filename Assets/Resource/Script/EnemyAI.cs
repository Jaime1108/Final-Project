using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private Animator animator;
    // Enemy State Variables
    float sightRange =20f, closeRangeDetection = 3f, attackRange = 2f;
    public bool playerInSight, playerInAttack;
    private Quaternion targetRotation;
    PlayerStat playerstat;
    private float timeSinceLastAttack = 0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerstat = player.GetComponent<PlayerStat>();

    }

    private void Update()
    {
        playerInSight = CanSeePlayer() || Vector3.Distance(transform.position, player.position) <= closeRangeDetection;
        playerInAttack = Vector3.Distance(transform.position, player.position) <= attackRange;
        timeSinceLastAttack += Time.deltaTime;
        if (playerInAttack && playerInSight && timeSinceLastAttack >= 3.6f){
            AttackPlayer();
        }
        else if (playerInSight){
            ChasePlayer();
        }
        else{
            Patrol();
        }
    }

    private void Patrol()
    {
        
        animator.SetBool("IsMoving", false);
        // Implement patrol logic here
        Debug.Log("patrolling");
        LookAround();

    }

    private void ChasePlayer()
    {
        
        animator.SetBool("IsMoving", true);
        
        
        agent.SetDestination(player.position);
        Debug.Log("chasing");
    }

    private void AttackPlayer()
    {   Debug.Log("Attacking Player!");
        // Stop moving when attacking
        agent.SetDestination(transform.position);
        // Attack logic (e.g., play attack animation)
        animator.SetTrigger("Attack");
        // Attack logic (e.g., play attack animation)
        playerstat.TakeDamage(10); // Dealing 10 damage to the player
        Debug.Log("Player takes 10 damage!");
        timeSinceLastAttack = 0f;
    }
    private bool CanSeePlayer(){
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        
        if (angle < 50f) // 50 degrees on both sides
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
            {
                return hit.transform == player; // Check if the player is actually seen
            }
        }
        return false;
    }

    private void LookAround(){
        
        float randomAngle = Random.Range(-60f, 60f);
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
        
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 leftLimit = Quaternion.Euler(0, -50, 0) * transform.forward * sightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 50, 0) * transform.forward * sightRange;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }
}
