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
    //public GameObject weaponPrefab;
    public WeaponType type;
    public string attackTrigger;
}
