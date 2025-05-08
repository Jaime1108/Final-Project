using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    public Material characterMaterial;
    public GameObject capeObject;
    

    private void Start(){
        
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
