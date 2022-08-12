using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupEquipmentItemDetail : MonoBehaviour
{
    public static PopupEquipmentItemDetail instance;
    [SerializeField]
    private GameObject PlaceItemImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceItemName;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceAbility;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceValue;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceReinforcePercent;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceSellCoin;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceUpgradeCoin;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceReinForceLevel;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceEquipText;
    [SerializeField]
    private GameObject SuccessParticle;
    [SerializeField]
    private GameObject FailParticle;

    public UserItemData thisItem;
    public MyItems thisItemObject;

    public GameObject popupInfo;

    protected private int sellCoinValue = 10;
    protected private int upgradeCoinValue = 10;
    public float rangePercent = 1.0f;
    // Start is called before the first frame update
    public void Awake()
    {
        instance = this;
        Hide();
    }
    public void OnEnable()
    {
    }
    public void Show()
    {
        SetCanvas();
        this.gameObject.SetActive(true);
    }
    public void Hide() => this.gameObject.SetActive(false);
    public void SetCanvas()
    {
        PlaceItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Icons/{0}/{1}", thisItem.type, thisItem.ItemName));
        PlaceItemName.text = thisItem.ItemName;
        switch (thisItem.type)
        {
            case "Weapon":
                PlaceAbility.text = "Attack Damage";
                break;
            case "Helmet":
                PlaceAbility.text = "Attack Speed";
                break;
            case "Armor":
                PlaceAbility.text = "Health";
                break; 
            case "Shoes":
                PlaceAbility.text = "Speed";
                break;
            default:
                PlaceAbility.text = thisItem.type+"ERROR";
                break;
        }
        PlaceValue.text = thisItem.value.ToString("+0.##;-0.##;0");
        sellCoinValue = thisItem.reinForceLevel * 10;
        PlaceSellCoin.text = sellCoinValue.ToString();
        upgradeCoinValue = thisItem.reinForceLevel * 15;
        PlaceUpgradeCoin.text = upgradeCoinValue.ToString();
        PlaceReinForceLevel.text = "LV."+thisItem.reinForceLevel.ToString();

        rangePercent = this.thisItem.reinForceLevel < 20 ? 1 - (this.thisItem.reinForceLevel * 0.05f) : 0.05f;
        PlaceReinforcePercent.text = ((1.0f - rangePercent) * 100).ToString("F0")+"%";
        PlaceEquipText.text = thisItem.isEquip ? "UnEquip" : "Equip";

    }
    public void SellItemButton()
    {
        SoundManager.Instance.PlayBTNSound("Pickup_Gold_02");
        if (thisItem.isEquip) // 장착중이면 해제 후 제거
        {
            EquipItemButton();
        }
        DataManager.instance.UserRemoveItem(thisItemObject.ItemIndex);
        EquipController.instance.characterDatas.Coin += sellCoinValue;
        DataManager.instance.Save(EquipController.instance.characterDatas);
        EquipController.instance.SaveData();
        ReloadItem();
        Hide();
    }
    public void UpgradeItemButton()
    {
        if (EquipController.instance.characterDatas.Coin >= upgradeCoinValue)
        {
            Debug.Log("Upgrade");
            EquipController.instance.characterDatas.Coin -= upgradeCoinValue;

            DataManager.instance.Save(EquipController.instance.characterDatas);

            float tempRand = Random.Range(0f, 1f);
            rangePercent = this.thisItem.reinForceLevel < 20 ? 1 - (this.thisItem.reinForceLevel * 0.05f) : 0.05f;
            PlaceReinforcePercent.text = ((1.0f - rangePercent) * 100).ToString("F0") + "%";

            if (tempRand <= rangePercent)//강화성공
            {
                SuccessParticle.SetActive(false);
                SuccessParticle.SetActive(true);
                SoundManager.Instance.PlayBTNSound("JustImpacts-Extension2_Metal_Hit_Crash_200");
                Debug.Log("강화완료");
                if (thisItem.isEquip) // 장착중이면 해제 후 업그레이드 후 재장착
                {
                    EquipItemButton();
                    thisItem.value *= 1.1f; //능력치 10%상승
                    thisItem.reinForceLevel++;
                    thisItemObject.reinForceLevelText.text = "LV." + thisItem.reinForceLevel.ToString();
                    EquipItemButton();
                }
                else
                {
                    thisItem.reinForceLevel++;
                    thisItemObject.reinForceLevelText.text = "LV." + thisItem.reinForceLevel.ToString();
                    thisItem.value *= 1.1f;
                }
                DataManager.instance.UserChangeItem(thisItem, thisItemObject.ItemIndex);
                EquipController.instance.SaveData();

                thisItemObject.OpenEquipDeatilCanvas();
            }
            else            //강화 실패(파괴)
            {
                SoundManager.Instance.PlayBTNSound("Just_Impacts_Extension-I_171");
                if (thisItem.isEquip) // 장착중이면 해제 
                    EquipItemButton();

                DataManager.instance.UserRemoveItem(thisItemObject.ItemIndex);
                DataManager.instance.Save(EquipController.instance.characterDatas);
                EquipController.instance.SaveData();
                Debug.Log("파괴됨");
                ReloadItem();
                Hide();
                FailParticle.SetActive(false);
                FailParticle.SetActive(true);
                
            }
        }
        else
        {
            //코인부족
            var popup = Instantiate(popupInfo.gameObject, this.transform);
            popup.GetComponent<PopupInfo>().infoText.text = "You don't have enough coins.";
        }

    }
    public void ReloadItem()
    {
        for (int i = 0; i < EquipController.instance.Inventory.childCount; i++)
        {
            Destroy(EquipController.instance.Inventory.GetChild(i).gameObject);
        }
        EquipController.instance.SetItems();
    }
    public void EquipItemButton()
    {
        thisItemObject.Equip();
        thisItemObject.EquipCheck();
        PlaceEquipText.text = thisItem.isEquip ? "UnEquip" : "Equip";
    }

}
