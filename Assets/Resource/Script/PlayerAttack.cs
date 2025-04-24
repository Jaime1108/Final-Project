using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] weapons; // Assign inactive weapons in Inspector
    private int currentWeaponIndex = 0;
    public GameObject weaponHolder; // Parent object in right hand
    private GameObject currentWeapon;
    private WeaponData equippedWeapon;
    private Animator animator;

    private bool isEquipped = false; // NEW: Track if weapon is currently active

    void Start()
    {
        animator = GetComponent<Animator>();
        EquipWeapon(currentWeaponIndex); // Equip default at start
    }

    void Update()
    {
        HandleAttackInput();
        HandleWeaponSwitchInput();
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && isEquipped)
        {
            Attack();
        }
    }

    void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeaponIndex == 0 && isEquipped)
            {
                UnequipWeapon(); // Double-tap = sheathe
            }
            else
            {
                currentWeaponIndex = 0;
                EquipWeapon(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons.Length > 1)
            {
                if (currentWeaponIndex == 1 && isEquipped)
                {
                    UnequipWeapon();
                }
                else
                {
                    currentWeaponIndex = 1;
                    EquipWeapon(1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            EquipWeapon(currentWeaponIndex);
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
        {
            Debug.LogWarning("Invalid weapon index");
            return;
        }

        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        currentWeapon = weapons[index];
        currentWeapon.SetActive(true);
        isEquipped = true;
        WeaponData weaponData = currentWeapon.GetComponent<WeaponData>();
        if (weaponData.type == WeaponType.OneHanded){
            animator.SetBool("HasShortSword", true);
        }
        else
        {
            animator.SetBool("HasShortSword", false);
        }
        

        equippedWeapon = currentWeapon.GetComponent<WeaponData>();
        if (equippedWeapon == null)
        {
            Debug.LogError("WeaponData is missing on " + currentWeapon.name);
            return;
        }

        Debug.Log("Equipped: " + equippedWeapon.weaponName);
    }

    void UnequipWeapon()
    {
        WeaponData weaponData = currentWeapon.GetComponent<WeaponData>();
        if (weaponData.type == WeaponType.OneHanded){
            animator.SetBool("HasShortSword", false);
        }
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
            equippedWeapon = null;
            isEquipped = false;
            Debug.Log("Weapon sheathed.");
        }
    }

    void Attack()
    {
        if (equippedWeapon == null) return;

        animator.SetTrigger(equippedWeapon.attackTrigger);
        Debug.Log("Attacking with " + equippedWeapon.weaponName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null && equippedWeapon != null)
            {
                enemy.TakeDamage(equippedWeapon.damage);
                Debug.Log("Hit enemy! Dealt " + equippedWeapon.damage + " damage.");
            }
        }
    }
}
