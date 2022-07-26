using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MyItems : MonoBehaviour
{
    public UserItemData itemData;
    public int ItemIndex;
    public GameObject Check;
    public GameObject ItemImageObject;

    public TMPro.TextMeshProUGUI reinForceLevelText;

    int varNum = 0;
    public void Awake()
    {
        Check.SetActive(itemData.isEquip);
    }
    public void Equip()
    {
        CharacterDatas character = DataManager.instance.Load();
        itemData.isEquip = itemData.isEquip ? false : true;

        Check.SetActive(itemData.isEquip);

        switch (itemData.type)
        {
            case "Weapon":
                character.AD += itemData.isEquip ? (int)itemData.value:-(int)itemData.value;
                varNum = (int)Equipment.Weapon;
                break; 
            case "Helmet":
                character.AS += itemData.isEquip ? itemData.value : -itemData.value; 
                varNum = (int)Equipment.Helmet;
                break;
            case "Armor":
                character.HP += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                varNum = (int)Equipment.Armor;
                break;
            case "Shoes":
                character.Speed += itemData.isEquip ? (int)itemData.value : -(int)itemData.value;
                varNum = (int)Equipment.Shoes;
                break;
            default:
                Debug.Log("NoneItemType");
                break;
        }

        DataManager.instance.UserChangeItem(itemData, ItemIndex);
        DataManager.instance.Save(character);
        EquipController.SaveAndReferesh();
    }
    public void EquipCheck()
    {
        if (itemData.isEquip) // ¿Â¬¯
        {
            if (null != EquipController.instance.equipData.EquipmentInfo[varNum])
                EquipController.instance.equipData.EquipmentInfo[varNum].GetComponent<MyItems>().Equip(); // ∞∞¿∫ ≈∏¿‘ ¿Â¬¯«ÿ¡¶
            EquipController.instance.equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", itemData.type, itemData.ItemName));
            EquipController.instance.equipData.EquipmentPlace[varNum].SetActive(true);
            EquipController.instance.equipData.EquipmentInfo[varNum] = this.gameObject;
        }
        else // ¿Â¬¯ «ÿ¡¶
        {
            EquipController.instance.equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = null;
            EquipController.instance.equipData.EquipmentPlace[varNum].SetActive(false);
            EquipController.instance.equipData.EquipmentInfo[varNum] = null;
        }
    }
    public void OpenEquipDeatilCanvas()
    {
        PopupEquipmentItemDetail.instance.thisItemObject = this;
        PopupEquipmentItemDetail.instance.thisItem = itemData;
        PopupEquipmentItemDetail.instance.Show();
    }
}
