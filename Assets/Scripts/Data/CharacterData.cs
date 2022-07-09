using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CharacterDatas
{
    public float AD;
    public float AP;
    public float AS;
    public float Speed;
    public int HP;
    public int Coin;
}
[System.Serializable]
public class UserItemData
{
    public int ItemIndex;
    public string ItemName;
    public string type;
    public int reinForceLevel;
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
    public UserItemDatas userItemDatas;
    string path;
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        path = Application.dataPath+"/Data";
#elif UNITY_ANDROID
        path = Application.persistentDataPath;
#endif

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
        characters = new CharacterDatas { AD = 10, AP = 0, AS = 1.5f, Speed = 3, HP = 100, Coin = 0 };
        Save();
    }
    public void Save()
    {
        Debug.Log("save");
        string CharacterDatas = JsonUtility.ToJson(characters);
        File.WriteAllText(path + "/CharacterData.json", CharacterDatas);
    }
    public void Save(CharacterDatas characters)
    {
        Debug.Log("save");
        string CharacterDatas = JsonUtility.ToJson(characters);
        File.WriteAllText(path + "/CharacterData.json", CharacterDatas);
    }
    public CharacterDatas Load()
    {
        try
        {
            characters = JsonUtility.FromJson<CharacterDatas>(File.ReadAllText(path+ "/CharacterData.json"));
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
        try
        {
            userItemDatas = JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((path + "/UserItemData.json")));
        }
        catch (FileNotFoundException f)
        {
            NewItemData();
            ItemDatasLoad();
        }
        return userItemDatas;
    }
    public void NewItemData()
    {
        userItemDatas = new UserItemDatas { ItemRows = { new UserItemData { ItemIndex = 0, ItemName = "Weapon0", type = "Weapon", value = 5.0f, isEquip = false } ,
                                                         new UserItemData { ItemIndex = 1, ItemName = "Armor0", type = "Armor", value = 5.0f, isEquip = false }  ,
                                                         new UserItemData { ItemIndex = 2, ItemName = "Helmet0", type = "Helmet", value = 5.0f, isEquip = false } ,
                                                         new UserItemData { ItemIndex = 3, ItemName = "Helmet1", type = "Helmet", value = 10.0f, isEquip = false } ,
                                                         new UserItemData { ItemIndex = 4, ItemName = "Helmet2", type = "Helmet", value = 15.0f, isEquip = false } ,
                                                         new UserItemData { ItemIndex = 5, ItemName = "Shoes0", type = "Shoes", value = 5.0f, isEquip = false }}
        };
        UserItemDataSave(userItemDatas);
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
        File.WriteAllText(path + "/UserItemData.json", itemDatas);
    }
    public void UserItemDataSave(string itemDatas)
    {
        Debug.Log("ItemSave");
        File.WriteAllText(path + "/UserItemData.json", itemDatas);
    }
}
