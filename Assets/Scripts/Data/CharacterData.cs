using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

class CharacterDatas
{
    public int AD;
    public int AP;
    public int AS;
    public int Speed;
    public int Coin;

}
public class CharacterData : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterDatas characters = new CharacterDatas{ AD=100, AP=0, AS=3, Speed=35, Coin=0};
    void Start()
    {
        //Save();
        Load();
    }
    public void Save()
    {
        Debug.Log("save");
        string CharacterDatas = JsonUtility.ToJson(characters);
        File.WriteAllText(Application.dataPath + "/Data/CharacterData.json", CharacterDatas);
    }
    public void Load()
    {
        Debug.Log("load");
        CharacterDatas characters2 = JsonUtility.FromJson<CharacterDatas>(File.ReadAllText((Application.dataPath + "/Data/CharacterData.json")));
        Debug.Log(characters2.Speed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
