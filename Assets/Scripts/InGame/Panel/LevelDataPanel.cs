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
    private TextMeshProUGUI MyStars;
    [SerializeField]
    private TextMeshProUGUI AllStars;
    [SerializeField]
    private GameObject popupInfo;


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
            if (i != 0 && levelData.levelInfo[i - 1] >= 2) //���� �������� 2��Ÿ�̻����� Ŭ����
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
    public void onClickLock()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        var popup = Instantiate(popupInfo.gameObject,this.transform);
        popup.GetComponent<PopupInfo>().infoText.text = "You must clear the previous step at least 2 stars.";
    }
    public void onClickNextPage()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        var popup = Instantiate(popupInfo.gameObject, this.transform);
        popup.GetComponent<PopupInfo>().infoText.text = "You need 50 stars.";
    }
}
