using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using ThirteenPixels.Soda;
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
    private Vector3 MapPos;
    private GameObject curMap;

    private void Awake()
    {
        instance = this;
        MapPos = BaseMap.transform.position;
    }
    private void Start()
    {
        GenerateMapWithNavmesh();
        GameManager.instance.StartNextStage();
        enemySpawner.componentCache.SpawnEnemies();
        gameState.value = GameState.STARTED;
    }
    private void GenerateMapWithNavmesh(){
        if(GameManager.instance.gameData.nowProgressLevel != 0 && 
            GameManager.instance.gameData.nowProgressLevel % 10 == 0){
            //boss stage
            curMap = BossMaps[Random.Range(0, BossMaps.Length)];
        }
        else{  //normal stage   (need add special stage - if needed)
            curMap = normalMaps[Random.Range(0, normalMaps.Length)];
        }
        curMap.transform.position = MapPos;
        curMap.SetActive(true);
        
        NavMeshSurface navsurface = curMap.GetComponentInChildren<NavMeshSurface>();
        navsurface.RemoveData();
        navsurface.BuildNavMesh();

        
    }

    public void nextStage()
    {   //다음 스테이지 값 수정 위치
        GameManager.instance.gameData.nowProgressLevel++;

        //다음스테이지 나올 몬스터 ( 레벨 및 구현 필요 또는 랜덤? )
        GameManager.instance.gameData.EnemySet = new List<string>() { "Mad Flower", "eyebat" };

        gameState.value = GameState.INIT;
        SceneManager.LoadScene("GameScene");
    }
    public void GoMain()
    {
        GameManager.instance.gameData.nowProgressLevel = 0;
        GameManager.instance.gameData.DeadCount = 0;
        gameState.value = GameState.INIT;
        SceneManager.LoadScene("MainScene");
    }
    public void OpenContinuePannel()
    {
        continuePannel.SetActive(true);
    }
    public void Continue()
    {
        GameManager.instance.player.gameObject.SetActive(true);
        GameManager.instance.gameData.nowHP = 30;
        SceneManager.LoadScene("GameScene");
    }
}
