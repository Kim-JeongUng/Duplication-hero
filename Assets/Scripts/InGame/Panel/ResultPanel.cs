using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public GameObject RewardFrame;
    public Transform ItemsParent;
    public TextMeshProUGUI coins;
    // Start is called before the first frame update
    public void OnEnable()
    {
        SetPanel();
    }
    public void SetPanel()
    {
        coins.text = GameManager.instance.gameData.acquiredCoins.ToString();
        for (int i = 0; i < GameManager.instance.gameData.acquiredItems.Count; i++)
        {
            var item = Instantiate(RewardFrame, ItemsParent);
            item.GetComponent<RewardFrame>().Icon.sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", GameManager.instance.gameData.acquiredItems[i].type, GameManager.instance.gameData.acquiredItems[i].ItemName));
        }
    }
}
