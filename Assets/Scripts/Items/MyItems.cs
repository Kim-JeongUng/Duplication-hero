using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MyItems : MonoBehaviour
{
    public UserItemData itemData;
    
    public void Awake()
    {
    }
    public void Equip()
    {
        CharacterDatas character = CharacterData.instance.Load();
        
        switch (itemData.type)
        {
            case "Weapon":
                character.AD += itemData.isEquip ? (int)itemData.value:-(int)itemData.value;
                break; 
            case "Helmet":
                character.AS += itemData.isEquip ? itemData.value : -itemData.value;
                break;
            case "Armor":
                character.HP += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                break;
            case "Shoes":
                character.Speed += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                break;
            default:
                Debug.Log("NoneItemType");
                break;
        }

        CharacterData.instance.Save(character);
    }
}
