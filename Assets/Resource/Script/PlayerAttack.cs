using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] weapons= new GameObject[2]; // Assign inactive weapons in Inspector
    private int currentWeaponIndex = 0;
    public GameObject weaponHolder; // Parent object in right hand
    private GameObject currentWeapon;
    private WeaponData equippedWeapon;
    private Animator animator;

    private bool isEquipped = false; // NEW: Track if weapon is currently active
    public bool oneHandWeapon = false;
    public bool twoHandWeapon = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Instantiatedefault weapons into weaponHolder
        GameObject weapon = Instantiate(weapons[0], weaponHolder.transform);
        weapons[0] = weapon;
        weapon.SetActive(false);

    

        EquipWeapon(currentWeaponIndex); // Equip default at start
    }

    void Update()
    {
        HandleAttackInput();
        HandleWeaponSwitchInput();
        animator.SetBool("HasShortSword", oneHandWeapon);
        animator.SetBool("HasLongSword", twoHandWeapon);
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && isEquipped)
        {
            animator.SetTrigger("LightAttack");
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

    public void PickupWeapon(WeaponData newWeaponData){
        int slotIndex = 0;
        if (newWeaponData.type == WeaponType.OneHanded){
            slotIndex = 0; // One-handed weapons go to slot 0
        }else{
            slotIndex = 1;
        }
         if (slotIndex >= weapons.Length){
            Debug.LogError("Weapons array is too small for slot " + slotIndex);
            return;
        }

        // if there’s already a weapon in this slot, remove it
        if (weapons[slotIndex] != null){
            weapons[slotIndex] = null;
        }

        // instantiate the new weapon and parent it to the weaponHolder
        GameObject newWeapon = Instantiate(newWeaponData.Sword, weaponHolder.transform);
        newWeapon.SetActive(false);

        // Assign it to the correct slot
        weapons[slotIndex] = newWeapon;

        Debug.Log($"Picked up {newWeaponData.weaponName} and placed in slot {slotIndex}");
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
        if(weaponData.type == WeaponType.OneHanded){
            oneHandWeapon = true;
            twoHandWeapon = false;
            
        }
        else if(weaponData.type == WeaponType.TwoHanded) {
            oneHandWeapon = false;
            twoHandWeapon = true;
            
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
            oneHandWeapon =  false;
        }
        else{
            twoHandWeapon = false;
        }
        if (currentWeapon != null){
            currentWeapon.SetActive(false);
            equippedWeapon = null;
            isEquipped = false;
            Debug.Log("Weapon sheathed.");
        }
    }

    void Attack(string AttackType)
    {
        if (equippedWeapon == null) return;

        
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
        else if (other.CompareTag("Boss")){
            BossAIScript boss  = other.GetComponent<BossAIScript>();
            if (boss  != null && equippedWeapon != null)
            {
                boss.TakeDamage(equippedWeapon.damage);
                Debug.Log("Hit boss! Dealt " + equippedWeapon.damage + " damage.");
            }
        }
        
    }
}
