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
    public string ItemName;
    public string type;
    public int reinForceLevel;
    public float value;
    public bool isEquip;
}
[System.Serializable]
public class LevelData
{
    public int[] levelInfo;
}
[System.Serializable]
public class UserItemDatas
{
    public List<UserItemData> ItemRows;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public CharacterDatas characters;
    private UserItemDatas userItemDatas;
    private UserItemDatas AllItemDatas;
    private LevelData levelData;
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
        AllItemPresetLoad();
        Load();
        ItemDatasLoad();
        LoadLevelData();
    }
    public void DeleteAllData()
    {
        File.SetAttributes(path, FileAttributes.Normal); //폴더 읽기 전용 해제

        foreach (string _folder in Directory.GetDirectories(path)) //폴더 탐색
        {
            DeleteAllData(); //재귀 호출
        }

        foreach (string _file in Directory.GetFiles(path)) //파일 탐색
        {
            File.SetAttributes(_file, FileAttributes.Normal); //파일 읽기 전용 해제
            File.Delete(_file); //파일 삭제
        }

        Directory.Delete(path); //폴더 삭제
        Directory.CreateDirectory(path);
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
            string CharacterData = Resources.Load<TextAsset>("PresetData/CharacterData").ToString();
            File.WriteAllText(path + "/CharacterData.json", CharacterData);
            characters = JsonUtility.FromJson<CharacterDatas>(File.ReadAllText(path + "/CharacterData.json"));
        }
        return characters;
    }
    public UserItemDatas ItemDatasLoad()
    {
        try
        {
            userItemDatas = JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((path + "/UserItemData.json")));
            return userItemDatas;
        }
        catch (FileNotFoundException f)
        {
            string UserItemData = Resources.Load<TextAsset>("PresetData/UserItemData").ToString();
            File.WriteAllText(path + "/UserItemData.json", UserItemData);
            userItemDatas = JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((path + "/UserItemData.json")));
            return userItemDatas;
        }
    }
    public LevelData LoadLevelData()
    {
        LevelData TempLevel;
        try
        {
            TempLevel = JsonUtility.FromJson<LevelData>(File.ReadAllText((path + "/LevelData.json")));
        }
        catch (FileNotFoundException f)
        {
            string Data = Resources.Load<TextAsset>("PresetData/LevelData").ToString();
            File.WriteAllText(path + "/LevelData.json", Data);
            TempLevel = JsonUtility.FromJson<LevelData>(File.ReadAllText((path + "/LevelData.json")));
        }
        levelData = TempLevel;
        return levelData;
    }
    public void ChangeLevelData(int nowStageLevel, int getStars) { 
        levelData.levelInfo[nowStageLevel] = levelData.levelInfo[nowStageLevel] > getStars ? levelData.levelInfo[nowStageLevel] : getStars;
        string LevelDatas = JsonUtility.ToJson(levelData);
        File.WriteAllText(path + "/LevelData.json", LevelDatas);
    }

    public void UserGetItem(UserItemData NewItem)
    {
        Debug.Log("ItemAdd");
        UserItemDatas UserItem = ItemDatasLoad();
        UserItem.ItemRows.Add(NewItem);
        UserItemDataSave(UserItem);
    }
    public void UserGetItem(UserItemData NewItem, int ItemIndex)
    {
        Debug.Log("ItemAdd");
        UserItemDatas UserItem = ItemDatasLoad();
        UserItem.ItemRows.Add(NewItem);
        UserItemDataSave(UserItem);
    }
    public void UserRemoveItem(int ItemIndex)
    {
        Debug.Log("ItemRemove");
        UserItemDatas UserItem = ItemDatasLoad();
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
        Debug.Log("NewItemSave");
        File.WriteAllText(path + "/UserItemData.json", itemDatas);
    }
    public UserItemDatas AllItemPresetLoad()
    {
        try
        {
            AllItemDatas = JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((path + "/AllItemData.json")));
            return AllItemDatas;
        }
        catch (FileNotFoundException f)
        {
            string AllItemData = Resources.Load<TextAsset>("PresetData/AllItemData").ToString();
            File.WriteAllText(path + "/AllItemData.json", AllItemData);
            AllItemDatas = JsonUtility.FromJson<UserItemDatas>(File.ReadAllText((path + "/AllItemData.json")));
            return AllItemDatas;
        }
    }

    public UserItemData PickRandomItem()
    {
        UserItemData item;
        int rand = UnityEngine.Random.Range(0, AllItemDatas.ItemRows.Count);
        item = AllItemDatas.ItemRows[rand];
        return item;
    }
}
