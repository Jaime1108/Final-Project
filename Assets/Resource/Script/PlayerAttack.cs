using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] weapons= new GameObject[2]; // weapon slot
    private int currentWeaponIndex = 0;
    public GameObject weaponHolder; // Parent object in right hand
    private GameObject currentWeapon;
    private CharacterControl characterControl;
    private WeaponData equippedWeapon;
    private Animator animator;
    public UserInterface userInterface;

    private bool isEquipped = false; // NEW: Track if weapon is currently active
    public bool oneHandWeapon = false;
    public bool twoHandWeapon = false;
    private int lastAttackDamage = 0;


    //audio source
    public AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        animator = GetComponent<Animator>();
        characterControl = FindFirstObjectByType<CharacterControl>();
        // Instantiatedefault weapons into weaponHolder
        GameObject weapon = Instantiate(weapons[0], weaponHolder.transform);
        weapons[0] = weapon;
        weapon.SetActive(false);
        userInterface = FindFirstObjectByType<UserInterface>();
    

        EquipWeapon(currentWeaponIndex); // Equip default at start
        userInterface.weaponDamage.text = "";
    }

    void Update()
    {
        if(characterControl.isAlive){
            HandleAttackInput();
            HandleWeaponSwitchInput();
            animator.SetBool("HasShortSword", oneHandWeapon);
            animator.SetBool("HasLongSword", twoHandWeapon);
    }
        }
        

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && isEquipped){
            animator.SetTrigger("LightAttack");
            lastAttackDamage = equippedWeapon.damage; 
            Invoke(nameof(LightAttackSFX),0.2f);
        }else if (Input.GetMouseButtonDown(1) && isEquipped){
            animator.SetTrigger("HeavyAttack");
            lastAttackDamage = equippedWeapon.damage + equippedWeapon.damage/2 ;
            if( audioManager !=null){
                if(twoHandWeapon){
                Invoke(nameof(HeavyAttackSFX),0.7f);
                }
                else{
                    Invoke(nameof(LightAttackSFX),0.4f);
                }
            };
            
        }

    }
    void HeavyAttackSFX(){
        
        audioManager.PlaySFX("Heavy Attack");
    }

    void LightAttackSFX(){
        audioManager.PlaySFX("Sword Swing");
    }

    void HandleWeaponSwitchInput(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            if (weapons[0] != null){
                if (currentWeaponIndex == 0 && isEquipped)
                {
                    UnequipWeapon();}// double-tap = sheathe
                else{
                    currentWeaponIndex = 0;
                    EquipWeapon(0);
                }
            }
            else{
                UnequipWeapon();}
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            if (weapons.Length > 1 && weapons[1] != null){
                if (currentWeaponIndex == 1 && isEquipped){
                    UnequipWeapon();}
                else{
                    currentWeaponIndex = 1;
                    EquipWeapon(1);}
            }else{
                Debug.Log("No weapon in slot 2!");
                UnequipWeapon();}
        }

        if (Input.GetKeyDown(KeyCode.Tab)){
            int nextIndex = (currentWeaponIndex + 1) % weapons.Length;
            if (weapons[nextIndex] != null){
                currentWeaponIndex = nextIndex;
                EquipWeapon(nextIndex);
            }
            else{
                Debug.Log($"No weapon in slot {nextIndex + 1}!");
                UnequipWeapon();}
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
        if (index < 0 || index >= weapons.Length){
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
        userInterface.weaponDamage.text = "" + equippedWeapon.damage;
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
        userInterface.weaponDamage.text = "";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null && equippedWeapon != null)
            {
                enemy.TakeDamage(lastAttackDamage);
                audioManager.PlaySFX("Attack Skeleton");
                Debug.Log("Hit enemy! Dealt " + equippedWeapon.damage + " damage.");
            }
        }
        else if (other.CompareTag("Boss")){
            BossAIScript boss  = other.GetComponent<BossAIScript>();
            if (boss  != null && equippedWeapon != null)
            {
                boss.TakeDamage(lastAttackDamage);
                audioManager.PlaySFX("Boss Hit");
                Debug.Log("Hit boss! Dealt " + equippedWeapon.damage + " damage.");
            }
        }
        
    }
}
