using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public int health;
    public Transform player;
    private Animator animator;

    float sightRange = 20f, closeRangeDetection = 5f, attackRange = 3f;
    public bool playerInSight, playerInAttack;

    private Quaternion targetRotation;
    private PlayerStat playerstat;
    private float timeSinceLastAttack = 0f;
    private bool isAlive = true;
    public CorruptedCore corecorruption;

    // Patrol Variables
    private Vector3 originalPosition;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private float waitDuration = 5f;

    public int numberOfPatrolPoints = 4;
    public float patrolRadius = 15f;

    private bool isWaiting = false;
    private bool patrolInitialized = false;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerstat = player.GetComponent<PlayerStat>();
        health = 100;

        originalPosition = transform.position;

        // Generate random patrol points around original position
        for (int i = 0; i < numberOfPatrolPoints; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randomPoint = originalPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

            // Sample nearest point on NavMesh
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                patrolPoints.Add(hit.position);
            }
        }

        // Ensure at least 1 patrol point
        if (patrolPoints.Count == 0)
        {
            patrolPoints.Add(originalPosition);
        }
    }


    private void Update()
    {
        if (!isAlive) return;

        if (health == 0 || (corecorruption != null && corecorruption.isCleanse))
        {
            Die();
            return;
        }

        playerInSight = CanSeePlayer() || Vector3.Distance(transform.position, player.position) <= closeRangeDetection;
        playerInAttack = Vector3.Distance(transform.position, player.position) <= attackRange;
        timeSinceLastAttack += Time.deltaTime;

        if (playerInAttack && playerInSight && timeSinceLastAttack >= 3.6f)
        {
            AttackPlayer();
        }
        else if (playerInSight)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol(){
        if (!patrolInitialized)
        {
            if (!agent.isOnNavMesh) return;
            agent.SetDestination(patrolPoints[currentPatrolIndex]);
            patrolInitialized = true;
            animator.SetBool("IsMoving", true); // Start moving to first point
        }

        if (!isWaiting)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
            {
                // Arrived at patrol point
                isWaiting = true;
                waitTimer = 0f;
                agent.isStopped = true;

                // Stop movement animation, trigger idle
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsIdle", true); // Optional: only if you use an explicit "Idle" trigger
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitDuration)
            {
                // Done waiting
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;

                // Resume movement
                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(patrolPoints[currentPatrolIndex]);
                    agent.isStopped = false;
                    animator.SetBool("IsMoving", true);
                    animator.SetBool("IsIdle", false); // Optional: turn off Idle if used
                }
            }
        }
    }

    private void ChasePlayer()
    {
        if (!playerInAttack && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            animator.SetBool("IsMoving", true);
            agent.SetDestination(player.position);
            Debug.Log("Chasing player...");
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking Player!");
        animator.SetTrigger("Attack");
        playerstat.TakeDamage(10);
        Debug.Log("Player takes 10 damage!");
        timeSinceLastAttack = 0f;
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < 50f)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange))
            {
                return hit.transform == player;
            }
        }
        return false;
    }

    private void Die()
    {
        isAlive = false;
        animator.SetBool("Alive", false);
        animator.SetBool("IsMoving", false);
        agent.isStopped = true;
        Destroy(gameObject, 6f);
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;
        health -= damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 leftLimit = Quaternion.Euler(0, -50, 0) * transform.forward * sightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 50, 0) * transform.forward * sightRange;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }
}
