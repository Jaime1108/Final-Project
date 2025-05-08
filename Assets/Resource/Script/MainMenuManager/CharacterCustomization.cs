using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    public Material characterMaterial;
    public GameObject capeObject;
    public bool isOn = false;

    private void Start(){
        capeObject.SetActive(false);
    }
    public void ChangeArmorColor(Color color)
    {
        characterMaterial.color = color;
    }

    public void ToggleCape(bool isOn){
        Debug.Log(isOn);
        capeObject.SetActive(isOn);
    }
}
