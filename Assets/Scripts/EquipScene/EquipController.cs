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
    public GameObject WeaponPlace;
    public GameObject HelmetPlace;
    public GameObject ArmorPlace;
    public GameObject ShoesPlace;

    public GameObject Weapon;
    public GameObject Helmet;
    public GameObject Armor;
    public GameObject Shoes;
}
public class EquipController : MonoBehaviour
{
    public static EquipController instance;
    public UIText texts;
    public EquipData equipData;
    CharacterDatas characterDatas;
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
        for (int i=0; i<items.ItemRows.Count; i++)
        {
            var item = Instantiate(presetItem, Inventory);
            item.GetComponent<MyItems>().itemData = items.ItemRows[i];
            item.GetComponent<MyItems>().Check.SetActive(items.ItemRows[i].isEquip);
            item.GetComponent<MyItems>().ItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));

            //이미지 장착 스프라이트
            if(items.ItemRows[i].isEquip)
                equipData.ShoesPlace.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", items.ItemRows[i].type, items.ItemRows[i].ItemName));
        }
    }
    public void EquipPlayer()
    {
        // 텍스쳐 변경

        // 
    }
}
