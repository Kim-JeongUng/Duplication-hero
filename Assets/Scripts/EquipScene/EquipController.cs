using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class UIText
{
    public Text AD;
    public Text AP;
    public Text AS;
    public Text Speed;
    public Text HP;
    public Text Coin;
}
[System.Serializable]
public class EquipData
{
    public GameObject [] EquipmentPlace = new GameObject[4];
    public GameObject [] EquipmentInfo = new GameObject[4];
}
public enum Equipment
{
    Weapon,
    Helmet,
    Armor,
    Shoes
}
public class EquipController : MonoBehaviour
{
    public static EquipController instance;
    public UIText texts;
    public EquipData equipData;
    public CharacterDatas characterDatas;
    public static Action SaveAndReferesh;

    [SerializeField]
    private GameObject presetItem;
    [SerializeField]
    private Transform Inventory;

    // Start is called before the first frame update
    public void Awake()
    {
        instance = this;
        SaveAndReferesh = () => { SaveData(); };
        characterDatas =CharacterData.instance.Load();
        RefreshState();
        SetItems();
    }
    public void GoHome()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void SaveData(){
        characterDatas = CharacterData.instance.Load();
        RefreshState();
        CharacterData.instance.Save(characterDatas); 
    }
    public void RefreshState() {
        texts.AD.text = characterDatas.AD.ToString();
        texts.AP.text = characterDatas.AP.ToString();
        texts.AS.text = characterDatas.AS.ToString("F1");
        texts.Speed.text = characterDatas.Speed.ToString();
        texts.HP.text = characterDatas.HP.ToString();
        texts.Coin.text = characterDatas.Coin.ToString();
    }
    public void SetItems()
    {
        UserItemDatas items = CharacterData.instance.ItemDatasLoad();
        for (int i = 0; i < items.ItemRows.Count; i++)
        {
            var item = Instantiate(presetItem, Inventory);
            item.GetComponent<MyItems>().itemData = items.ItemRows[i];
            item.GetComponent<MyItems>().Check.SetActive(items.ItemRows[i].isEquip);
            item.GetComponent<MyItems>().ItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));

            //인벤토리 아이템 장착
            if (items.ItemRows[i].isEquip) { 
                int varNum = 0;
                switch (items.ItemRows[i].type)
                {
                    case "Weapon":
                        varNum = (int)Equipment.Weapon;
                        break;
                    case "Helmet":
                        varNum = (int)Equipment.Helmet;
                        break;
                    case "Armor":
                        varNum = (int)Equipment.Armor;
                        break;
                    case "Shoes":
                        varNum = (int)Equipment.Shoes;
                        break;
                    default:
                        Debug.Log("NoneType");
                        break;
                }
                if (null != equipData.EquipmentInfo[varNum])
                    equipData.EquipmentInfo[varNum].GetComponent<MyItems>().Equip(); // 같은 타입 장착해제
                equipData.EquipmentPlace[varNum].GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));
                equipData.EquipmentPlace[varNum].SetActive(true);
                equipData.EquipmentInfo[varNum] = item;
            }
        }
    }
    public void EquipPlayer()
    {
        // 텍스쳐 변경
        
    }
}
