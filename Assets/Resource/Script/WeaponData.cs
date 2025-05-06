using UnityEngine;
public enum WeaponType
{
    OneHanded,
    TwoHanded,
    SwordAndShield
}


public class WeaponData : MonoBehaviour
{
    public string weaponName;
    public int damage;
    public GameObject Sword;
    public WeaponType type;
    void Start()
    {
        // autoassign this GameObject to Sword
        Sword = this.gameObject;
    }

}
