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
    [SerializeField] private GameObject[] BaseMap;
    
    private GameObject curMap;
    private Vector3 CurMapPos;

    private void Awake() {
        
    }
    private void Start()
    {
        if(GameManager.instance.gameData.nowProgressLevel != 0 && 
            GameManager.instance.gameData.nowProgressLevel % 10 == 0){
            //boss stage
            curMap = BossMaps[Random.Range(0, BossMaps.Length)];
        }
        else{  //normal stage   (need add special stage - if needed)
            curMap = normalMaps[Random.Range(0, normalMaps.Length)];
        }
        CurMapPos = curMap.transform.position;
        curMap.SetActive(true);


        enemySpawner.componentCache.SpawnEnemies();
        gameState.value = GameState.STARTED;
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
