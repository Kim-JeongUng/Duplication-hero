using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PanelRoulette : MonoBehaviour
{
    public GameObject Roulette;
    public TextMeshProUGUI coinText;
    public int requireCoins = 500;
    public GameObject popupInfo;

    public GameObject infoPanel;

    public PopupItemDetail popupItemDetail;
    
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
        }

    }
    public void OnClickSpin()
    {
        //µ∑ ∫Ò±≥
        if (DataManager.instance.characters.Coin >= requireCoins)
        {
            //µ∑ ¿˙¿Â
            DataManager.instance.characters.Coin -= requireCoins;
            DataManager.instance.Save(DataManager.instance.characters);

            randNum = Random.Range(0, itemCount);
            //æ∆¿Ã≈€ ¿˙¿Â
            DataManager.instance.UserGetItem(RandItems[randNum]);

            //∑Í∑ø Ω√∞¢»ø∞˙
            StartCoroutine(SpinRoulette(randNum));
        }
        else
        {
            var popup = Instantiate(popupInfo.gameObject, this.transform);
            popup.GetComponent<PopupInfo>().infoText.text = "You don't have enough coins.";
        }
    }
    public IEnumerator SpinRoulette(int randNum)
    {
        Roulette.transform.eulerAngles = Vector3.zero;
        int i = 0;
        int targetAnguler = randNum * 45 + 720; // 2πŸƒ˚ µπ∞Ì ≈∏∞Ÿ ∞¢µµ±Ó¡ˆ
        float waitForSeconds = 0.01f;
        while (i < targetAnguler)
        {
            i += 5;
            Roulette.transform.eulerAngles = new Vector3(0, 0, i);
            if (i >= targetAnguler - 180)
            {
                waitForSeconds += 0.001f;
            }
            yield return new WaitForSeconds(waitForSeconds);
        }

        yield return new WaitForSeconds(0.5f);
        //∫∏ªÛ∆«≥⁄
        OpenRewardPanel();
    }
    public void OpenRewardPanel()
    {
        SetPopupPanel(randNum);
    }
    public void OpenInfoPanel(int index)
    {
        SetPopupPanel(index);
    }
    public void SetPopupPanel(int index)
    {
        infoPanel.SetActive(true);
        popupItemDetail.image.sprite = ItemImage[index].sprite;
        popupItemDetail.name.text = RandItems[index].ItemName;
        switch (RandItems[index].type)
        {
            case "Weapon":
                popupItemDetail.ability.text = "Attack Damage";
                break;
            case "Helmet":
                popupItemDetail.ability.text = "Attack Speed";
                break;
            case "Armor":
                popupItemDetail.ability.text = "Health";
                break;
            case "Shoes":
                popupItemDetail.ability.text = "Speed";
                break;
            default:
                popupItemDetail.ability.text = RandItems[index].type + "ERROR";
                break;
        }
        popupItemDetail.value.text = RandItems[index].value.ToString("+0.##;-0.##;0");
    }
    [System.Serializable]
    public struct PopupItemDetail
    {
        public TextMeshProUGUI name;
        public TextMeshProUGUI value;
        public TextMeshProUGUI ability;
        public Image image;
    }
}
