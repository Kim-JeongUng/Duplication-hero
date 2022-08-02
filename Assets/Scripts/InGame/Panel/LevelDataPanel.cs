using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDataPanel : MonoBehaviour
{
    private LevelData levelData;
    [SerializeField]
    private EachStage[] eachStages;
    [SerializeField]
    private TMPro.TextMeshProUGUI MyStars;
    [SerializeField]
    private TMPro.TextMeshProUGUI AllStars;

    // Start is called before the first frame update
    public void OnEnable()
    {
        levelData = DataManager.instance.LoadLevelData();
        SetPanel();
    }
    public void SetPanel()
    {
        int sumStars=0;
        for (int i = 0; i < levelData.levelInfo.Length; i++)
        {
            if (i != 0 && levelData.levelInfo[i - 1] == 3) //이전 스테이지 3스타로 클리어
            {
                eachStages[i].StageLock.SetActive(false);
            }
            for (int j = 0; j < levelData.levelInfo[i]; j++)
            {
                sumStars++;
                eachStages[i].StageStar[j].SetActive(true);
            }
        }
        MyStars.text = sumStars.ToString();
    }
}
