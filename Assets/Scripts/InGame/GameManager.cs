using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string nowSkillName = "";
    public bool haveSkill() => nowSkillName == "" ? false : true;

    public List<string> SkillNameSet = new List<string> { };
    public GameObject[] SkillResource;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        gameData = new GameData { nowSkillName = "", SkillNameSet = { "Fire", "Barrier", "Water" } };
        gameData.SkillResource = new GameObject[gameData.SkillNameSet.Count];
        for (int i = 0; i < gameData.SkillNameSet.Count; i++)
        {
            gameData.SkillResource[i] = Resources.Load<GameObject>(string.Format("SkillEffect/{0}", gameData.SkillNameSet[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
