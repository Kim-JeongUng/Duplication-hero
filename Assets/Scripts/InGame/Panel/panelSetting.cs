using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelSetting : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void onClickDELETE()
    {
        DataManager.instance.DeleteAllData();
    }
    public void onClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
