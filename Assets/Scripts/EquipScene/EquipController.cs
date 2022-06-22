using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EquipController : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoHome()
    {
        SceneManager.LoadScene("MainScene");
    }
}
