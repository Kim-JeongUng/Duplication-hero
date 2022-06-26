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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
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
            item.GetComponent<MyItems>().ItemImageObject.GetComponent<Image>().sprite = ResourceLoadSample();
        }
    }
    private Sprite ResourceLoadSample()
    {
        Debug.Log("ASD");
        Sprite texture;
        string PATH = "Icons/BanditLightArmor.png";    // 이미지 파일 패스를 써준다. 
                                              //중요한 것은 유니티 프로젝트 Assets/Resource/ 폴더 이후의 경로를 써주는 것이다. 이 폴더는 맘대로 바꿀 수가 없다.
        texture = Resources.Load(PATH, typeof(Sprite)) as Sprite;  // 이미지 로드
        return texture;  // 타겟 오브젝트에 메인 텍스쳐를 넣어준다.
    }
}
