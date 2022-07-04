using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string nowSkillName = "";
    public bool haveSkill() => nowSkillName == "" ? false : true;  // 현재 스킬을 보유하고있는지

    public List<string> SkillNameSet = new List<string> { };
    public GameObject[] SkillResource;
    public int nowProgressLevel;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    public Player player;

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
        gameData = new GameData { nowSkillName = "", SkillNameSet = { "Fire", "Barrier", "Water", "DarkDraw" }, nowProgressLevel = 0 };
        gameData.SkillResource = new GameObject[gameData.SkillNameSet.Count];
        ResourceLoad();
    }
    public void ResourceLoad()
    {
        for (int i = 0; i < gameData.SkillNameSet.Count; i++)
        {
            gameData.SkillResource[i] = Resources.Load<GameObject>(string.Format("SkillEffect/{0}", gameData.SkillNameSet[i]));
        }
    }
}
