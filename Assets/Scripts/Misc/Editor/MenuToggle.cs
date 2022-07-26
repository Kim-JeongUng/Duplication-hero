using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItems
{
    [MenuItem("DuplicationHero/EquipMent &1")]
    private static void goEquip()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/EquipmentScene.unity");
    }
    [MenuItem("DuplicationHero/MainScene &2")]
    private static void goMain()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
    }
    [MenuItem("DuplicationHero/GameScene &3")]
    private static void goGame()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
    }
}
public class MenuToggle : MonoBehaviour
{
    
}
