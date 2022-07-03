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

    int varNum = 0;
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
                varNum = 0;
                break; 
            case "Helmet":
                character.AS += itemData.isEquip ? itemData.value : -itemData.value; 
                varNum = 1;
                break;
            case "Armor":
                character.HP += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                varNum = 2;
                break;
            case "Shoes":
                character.Speed += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                varNum = 3;
                break;
            default:
                Debug.Log("NoneItemType");
                break;
        }

        CharacterData.instance.UserChangeItem(itemData, itemData.ItemIndex);
        CharacterData.instance.Save(character);
        EquipController.SaveAndReferesh();
    }
    public void EquipCheck()
    {
        if (itemData.isEquip)
        {
            if (null != EquipController.instance.equipData.EquipmentInfo[varNum])
                EquipController.instance.equipData.EquipmentInfo[varNum].GetComponent<MyItems>().Equip(); // 같은 타입 장착해제
            EquipController.instance.equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", itemData.type, itemData.ItemName));
            EquipController.instance.equipData.EquipmentInfo[varNum] = this.gameObject;
        }
        else
        {
            EquipController.instance.equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = null;
            EquipController.instance.equipData.EquipmentInfo[varNum] = null;
        }
    }
}
