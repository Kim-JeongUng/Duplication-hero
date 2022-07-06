using UnityEngine;
using UnityEngine.SceneManagement;
using ThirteenPixels.Soda;
using UnityEngine.AI;

public enum GameState
{
    INIT,
    STARTED
}

public class GameController : MonoBehaviour
{
    [SerializeField] GlobalGameState gameState;
    [SerializeField] private GlobalEnemySpawner enemySpawner;
    [SerializeField] private GameObject[] normalMaps;
    [SerializeField] private GameObject[] BossMaps;
    [SerializeField] private GameObject[] SpecialMaps;
    [SerializeField] private GameObject BaseMap;
    private Vector3 MapPos;
    private GameObject curMap;

    private void Awake() {
        MapPos = BaseMap.transform.position;
    }
    private void Start()
    {
        GenerateMapWithNavmesh();

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
        
        gameState.value = GameState.INIT;
        SceneManager.LoadScene("GameScene");
    }
    public void GoMain()
    {
        GameManager.instance.gameData.nowProgressLevel = 0;
        gameState.value = GameState.INIT;
        SceneManager.LoadScene("MainScene");
    }
}
