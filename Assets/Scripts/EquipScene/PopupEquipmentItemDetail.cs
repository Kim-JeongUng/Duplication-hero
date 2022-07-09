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
    private TMPro.TextMeshProUGUI PlaceSellCoin;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceUpgradeCoin;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceReinForceLevel;
    [SerializeField]
    private TMPro.TextMeshProUGUI PlaceEquipText;

    public UserItemData thisItem;
    public MyItems thisItemObject;

    protected private int sellCoinValue = 10;
    protected private int upgradeCoinValue = 10;

    // Start is called before the first frame update
    public void Awake()
    {
        instance = this;
        Hide();
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
        PlaceEquipText.text = thisItem.isEquip ? "UnEquip" : "Equip";

    }
    public void SellItemButton()
    {
        if (thisItem.isEquip) // 장착중이면 해제 후 제거
        {
            EquipItemButton();
        }
        CharacterData.instance.UserRemoveItem(thisItem.ItemIndex);
        EquipController.instance.characterDatas.Coin += sellCoinValue;
        CharacterData.instance.Save(EquipController.instance.characterDatas);
        EquipController.instance.SaveData();
        Destroy(thisItemObject);
        SceneManager.LoadScene("EquipScene");
    }
    public void UpgradeItemButton()
    {
        if (EquipController.instance.characterDatas.Coin >= upgradeCoinValue)
        {
            Debug.Log("Upgrade");
            EquipController.instance.characterDatas.Coin -= upgradeCoinValue;

            CharacterData.instance.Save(EquipController.instance.characterDatas);
            if (thisItem.isEquip) // 장착중이면 해제 후 업그레이드 후 재장착
            {
                EquipItemButton();
                thisItem.value *= 1.1f; //능력치 10%상승
                thisItem.reinForceLevel++;
                thisItemObject.reinForceLevelText.text = thisItem.reinForceLevel.ToString();
                EquipItemButton();
            }
            else
            {
                thisItem.reinForceLevel++;
                thisItemObject.reinForceLevelText.text = thisItem.reinForceLevel.ToString();
                thisItem.value *= 1.1f;
            }

            CharacterData.instance.UserChangeItem(thisItem, thisItem.ItemIndex);
            EquipController.instance.SaveData();
        }
        else
        {
            Debug.Log("돈없어요");
        }

        thisItemObject.OpenEquipDeatilCanvas();
    }
    public void EquipItemButton()
    {
        thisItemObject.Equip();
        thisItemObject.EquipCheck();
    }

}
