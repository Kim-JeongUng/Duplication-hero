using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using TMPro;
using UnityEngine.AI;

public enum GameState
{
    INIT,
    STARTED
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] GlobalGameState gameState;
    [SerializeField] private GlobalEnemySpawner enemySpawner;
    [SerializeField] private GameObject[] normalMaps;
    [SerializeField] private GameObject[] BossMaps;
    [SerializeField] private GameObject[] SpecialMaps;
    [SerializeField] private GameObject BaseMap;
    [SerializeField] private GameObject continuePannel;
    [SerializeField] private GameObject resultPannel;
    [SerializeField] private GameObject progressPannel;
    [SerializeField] private TextMeshProUGUI progressPannelText;
    [SerializeField] private GameObject bossPannel;
    private int finalStageNum = 5;
    private Vector3 MapPos;
    private GameObject curMap;

    private void Awake()
    {
        instance = this;
        MapPos = BaseMap.transform.position;
    }
    private void Start()
    {
        Init();
        GameManager.instance.gameData.EnemySet = new List<string>() { "Mad Flower", "eyebat", "Hapineko" };
        GameManager.instance.gameData.EnemyCount = 2;
        gameState.value = GameState.STARTED;


    }
    private void GenerateMapWithNavmesh()
    {
        if (GameManager.instance.gameData.isBossStage)
        {
            //boss stage
            curMap = BossMaps[Random.Range(0, BossMaps.Length)];
            GameManager.instance.gameData.EnemyCount = 1;
            //GameManager.instance.gameData.EnemySet = new List<string>() { "Dragon" };
            GameManager.instance.gameData.EnemySet = new List<string>() { "Dragon", "DogKnight" };
        }
        else
        {  //normal stage   (need add special stage - if needed)
            curMap = normalMaps[Random.Range(0, normalMaps.Length)];
        }
        curMap.transform.position = MapPos;
        curMap.SetActive(true);

        NavMeshSurface navsurface = curMap.GetComponentInChildren<NavMeshSurface>();
        navsurface.RemoveData();
        navsurface.BuildNavMesh();


    }
    public void Init()
    {
        GenerateMapWithNavmesh();
        GameManager.instance.StartNextStage();
        enemySpawner.componentCache.SpawnEnemies();
        gameState.value = GameState.INIT;
    }
    public void nextStage()
    {   //다음 스테이지 값 수정 위치
        GameManager.instance.gameData.nowProgressLevel++;
        if (GameManager.instance.gameData.nowProgressLevel != 0 && (GameManager.instance.gameData.nowProgressLevel + 1) % finalStageNum == 0)
            GameManager.instance.gameData.isBossStage = true;
        else
            GameManager.instance.gameData.isBossStage = false;
        //다음스테이지 나올 몬스터 ( 레벨 및 구현 필요 또는 랜덤? )
        GameManager.instance.gameData.EnemySet = new List<string>() { "Mad Flower", "eyebat", "Hapineko" };
        GameManager.instance.gameData.EnemyCount = 2;
        gameState.value = GameState.INIT;
        SceneManager.LoadScene("GameScene");
    }
    public void GoMain()
    {
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene("MainScene");
    }
    public void GetResultReward()
    {
        for (int i = 0; i < GameManager.instance.gameData.acquiredItems.Count; i++)
        {
            DataManager.instance.UserGetItem(GameManager.instance.gameData.acquiredItems[i]);
        }
        DataManager.instance.characters.Coin += GameManager.instance.gameData.acquiredCoins;
        DataManager.instance.Save();
    }
    public void OpenContinuePannel()
    {
        continuePannel.SetActive(true);
    }
    public void OpenResultPannel()
    {
        resultPannel.SetActive(true);
    }
    public void OpenProgressPannel()
    {
        progressPannelText.text = "Chapter " + (GameManager.instance.gameData.nowChapter + 1).ToString() + " - " + (GameManager.instance.gameData.nowProgressLevel + 1).ToString();
        progressPannel.SetActive(true);
    }
    public void CloseProgressPannel()
    {
        progressPannel.SetActive(false);
    }
    public void OpenBossPannel()
    {
        bossPannel.SetActive(true);
    }
    public void CloseBossPannel()
    {
        bossPannel.SetActive(false);
    }
    public void Continue()
    {
        GameManager.instance.player.GetComponent<Player>().Revive();
        GameManager.instance.player.gameObject.SetActive(true);

        StartCoroutine(GameManager.instance.player.GetComponent<Player>().Invincible());
    }
}
