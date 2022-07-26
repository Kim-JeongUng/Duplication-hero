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
        SceneManager.LoadScene("EquipmentScene");
    }
    public void GoGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
