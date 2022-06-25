using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CharacterDatas
{
    public int AD;
    public int AP;
    public float AS;
    public int Speed;
    public int HP;
    public int Coin;
}
[System.Serializable]
public class UserItemData
{
    public int ItemIndex;
    public string ItemName;
    public string type;
    public float value;
    public bool isEquip;
}

[System.Serializable]
public class UserItemDatas
{
    public List<UserItemData> ItemRows;
}

public class CharacterData : MonoBehaviour
{
    public static CharacterData instance;
    public CharacterDatas characters;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("HI");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        //Save();
        Load();
        ItemDatasLoad();
    }
    public void NewData()
    {
        characters = new CharacterDatas { AD = 100, AP = 0, AS = 1.5f, Speed = 3, HP = 100, Coin = 0 };
        Save();
    }
    public void Save()
    {
        Debug.Log("save");
        string CharacterDatas = JsonUtility.ToJson(characters);
        File.WriteAllText(Application.dataPath + "/Data/CharacterData.json", CharacterDatas);
    }
    public void Save(CharacterDatas characters)
    {
        Debug.Log("save");
        string CharacterDatas = JsonUtility.ToJson(characters);
        File.WriteAllText(Application.dataPath + "/Data/CharacterData.json", CharacterDatas);
    }
    public CharacterDatas Load()
    {
        try
        {
            characters = JsonUtility.FromJson<CharacterDatas>(File.ReadAllText((Application.dataPath + "/Data/CharacterData.json")));
        }
        catch (FileNotFoundException f)
        {
            NewData();
            Load();
        }
        return characters;
    }
    public UserItemDatas ItemDatasLoad()
    {
        return JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((Application.dataPath + "/Data/UserItemData.json")));
    }
    public void UserGetItem(UserItemData NewItem)
    {
        Debug.Log("ItemAdd");
        UserItemDatas UserItem = ItemDatasLoad();
        NewItem.ItemIndex = UserItem.ItemRows.Count;
        UserItem.ItemRows.Add(NewItem);
        UserItemDataSave(UserItem);
    }
    public void UserGetItem(UserItemData NewItem, int ItemIndex)
    {
        Debug.Log("ItemAdd");
        UserItemDatas UserItem = ItemDatasLoad();
        NewItem.ItemIndex = ItemIndex;
        UserItem.ItemRows.Add(NewItem);
        UserItemDataSave(UserItem);
    }
    public void UserRemoveItem(int ItemIndex)
    {
        //미완성
        Debug.Log("ItemRemove");
        UserItemDatas UserItem = ItemDatasLoad();
        //인덱스로 찾아야 하나? -> 아이템 하나 삭제 후 나머지 인덱스는 어떻게 처리?
        UserItem.ItemRows.Remove(UserItem.ItemRows[ItemIndex]);
        UserItemDataSave(UserItem);
    }
    public void UserChangeItem(UserItemData Item, int ItemIndex)
    {
        Debug.Log("ItemChange");
        UserItemDatas UserItem = ItemDatasLoad();
        UserItem.ItemRows[ItemIndex] = Item;
        UserItemDataSave(UserItem);
    }

    public void UserItemDataSave(UserItemDatas Items)
    {
        Debug.Log("ItemSave");
        string itemDatas = JsonUtility.ToJson(Items);
        File.WriteAllText(Application.dataPath + "/Data/UserItemData.json", itemDatas);
    }
    public void UserItemDataSave(string itemDatas)
    {
        Debug.Log("ItemSave");
        File.WriteAllText(Application.dataPath + "/Data/UserItemData.json", itemDatas);
    }
}
