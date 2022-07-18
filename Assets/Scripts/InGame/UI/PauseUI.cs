using UnityEngine;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI chapter;

    public void Pause()
    {
        Time.timeScale = 0;
        chapter.text = "CHAPTER "+(GameManager.instance.gameData.nowProgressLevel + 1).ToString();
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
