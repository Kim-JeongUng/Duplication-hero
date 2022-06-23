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
    // Update is called once per frame
    void Update()
    {
        
    }
}
