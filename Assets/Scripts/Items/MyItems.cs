using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MyItems : MonoBehaviour
{
    public string ItemName;

    [SerializeField]
    private string type;

    [SerializeField]
    public float value;

    public bool isEquip=> GetComponent<Toggle>().isOn;
    public void Awake()
    {
    }
    public void SetType(string type) => this.type = type;
    public void SetValue(int value) => this.value = value;

    public void Equip()
    {
        CharacterDatas character = CharacterData.instance.Load();
        
        switch (type)
        {
            case "Weapon":
                character.AD += isEquip ? (int)this.value:-(int)this.value;
                break; 
            case "Helmet":
                character.AS += isEquip ? this.value : -this.value;
                break;
            case "Armor":
                character.HP += isEquip ? (int)this.value : -(int)this.value;
                break;
            case "Shoes":
                character.Speed += isEquip ? (int)this.value : -(int)this.value;
                break;
            default:
                Debug.Log("NoneItemType");
                break;
        }

        CharacterData.instance.Save(character);
    }
}
