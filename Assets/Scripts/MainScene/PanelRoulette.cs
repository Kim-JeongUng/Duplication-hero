using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelRoulette : MonoBehaviour
{
    public GameObject Roulette;
    public Transform ItemsParent;
    public TextMeshProUGUI coinText;
    [SerializeField]
    private int requireCoins=100;

    private static int itemCount = 8;
    [SerializeField]
    private UserItemData[] RandItems = new UserItemData[itemCount];
    [SerializeField]
    private Image[] ItemImage = new Image[itemCount];
    private int randNum;

    // Start is called before the first frame update
    public void OnEnable()
    {
        SetPanel();
        //SoundManager.instance.PlaySFXSound("Jingle_Achievement_00");
    }
    public void SetPanel()
    {
        coinText.text = requireCoins.ToString();
        for (int i = 0; i < itemCount; i++)
        {
            RandItems[i] = DataManager.instance.PickRandomItem();
            ItemImage[i].sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", RandItems[i].type, RandItems[i].ItemName));
            //itemGo.GetComponent<presetItemdata>().itemData = Randitem;
        }
        
    }
    public void OnClickSpin()
    {
        //µ∑ ∫Ò±≥
        //µ∑ ¿˙¿Â
        randNum = Random.Range(0, 8);
        //æ∆¿Ã≈€ ¿˙¿Â

        //∑Í∑ø Ω√∞¢»ø∞˙
        StartCoroutine(SpinRoulette(randNum));
    }
    public IEnumerator SpinRoulette(int randNum)
    {
        int i = 0;
        int targetAnguler = randNum * 45 + 720; // 2πŸƒ˚ µπ∞Ì ≈∏∞Ÿ ∞¢µµ±Ó¡ˆ
        Debug.Log(randNum);
        while (i < targetAnguler)
        {
            Debug.Log("ASD");
            i+=5;
            Roulette.transform.eulerAngles = new Vector3(0, 0, i);
            yield return new WaitForSeconds(0.01f);
        }

        //∫∏ªÛ∆«≥⁄
    }
}
