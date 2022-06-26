using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MyItems : MonoBehaviour
{
    public UserItemData itemData;
    public GameObject Check;
    public GameObject ItemImageObject;


    public void Awake()
    {
        Check.SetActive(itemData.isEquip);
    }
    public void Equip()
    {
        CharacterDatas character = CharacterData.instance.Load();
        itemData.isEquip = itemData.isEquip ? false : true;

        Check.SetActive(itemData.isEquip);

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
            case "Boots":
                character.Speed += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                break;
            default:
                Debug.Log("NoneItemType");
                break;
        }
        CharacterData.instance.UserChangeItem(itemData, itemData.ItemIndex);
        CharacterData.instance.Save(character);
        EquipController.SaveAndReferesh();
    }
}
