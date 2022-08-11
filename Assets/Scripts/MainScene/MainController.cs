using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MainController : MonoBehaviour
{
    public void OnEnable()
    {
        if(GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
    }
    // Start is called before the first frame update
    public void GoEquipment()
    {
        SoundManager.instance.PlayBTNSound("Inventory_Open_01");
        SceneManager.LoadScene("EquipmentScene");
        //LoadingSceneController.Instance.LoadScene("EquipmentScene");
    }
    public void GoGame(int nowChapter = 0)
    {
        PlayerPrefs.SetInt("nowChapter", nowChapter);
        //SceneManager.LoadScene("GameScene");
        LoadingSceneController.Instance.LoadScene("GameScene");
    }
}
