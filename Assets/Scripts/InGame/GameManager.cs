using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string nowSkillName = "";
    public bool haveSkill() => nowSkillName == "" ? false : true;  // 현재 스킬을 보유하고있는지
    public List<string> EnemySet = new List<string> { };
    public int EnemyCount = 2;

    public List<string> SkillNameSet = new List<string> { };
    public GameObject[] SkillResource;
    public int nowHP;
    public int nowProgressLevel;
    public int DeadCount=0;
    public bool isBossStage = false;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;

    public EnemySpawner enemySpawner;
    public List<GameObject> EnemyPrefabs;

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
        gameData = new GameData { nowSkillName = "", SkillNameSet = { "Fire", "Barrier", "Water", "DarkDraw" ,"PulseShot","FireBreath","Healing"}, nowProgressLevel = 0 };
        gameData.SkillResource = new GameObject[gameData.SkillNameSet.Count];
        ResourceLoad();
    }
    public void LoadGameData(GameData gameData)
    {
        this.gameData = gameData; 
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
    public void StartNextStage() // 다음스테이지
    {
        enemySpawner.enemyCount = gameData.EnemyCount;
        //새로운 몬스터
        if (gameData.EnemySet.Count >= 1)
        {
            enemySpawner.enemies = new List<GameObject> { };
            foreach (GameObject EnemyPrefab in EnemyPrefabs)
            {
                if (gameData.EnemySet.Contains(EnemyPrefab.name))
                {
                    enemySpawner.enemies.Add(EnemyPrefab);

                }
            }
        }
        //새로운 스킬

        //리소스 로드
        ResourceLoad();
    }
}
