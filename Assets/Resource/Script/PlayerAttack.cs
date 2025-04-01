using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject sword;
    public int attackDamage = 20;
    private Animator animator;
    //private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        sword.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        //isAttacking = true;
        animator.SetTrigger("Attack");
        sword.SetActive(true);
        Debug.Log("Attacking!");
        //Invoke("DisableSword", 0.5f);
    }

 /*
    void DisableSword()
    {
        sword.SetActive(false);
        isAttacking = false;
    }
*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            Debug.Log("Enemy Health:" + enemy.health);
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Hit enemy! Dealt " + attackDamage + " damage.");
            }
        }
    }
}
