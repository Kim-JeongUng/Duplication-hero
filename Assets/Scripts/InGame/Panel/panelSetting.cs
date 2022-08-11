using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelSetting : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void onClickDELETE()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        DataManager.instance.DeleteAllData();
    }
    public void onClickQuit()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
