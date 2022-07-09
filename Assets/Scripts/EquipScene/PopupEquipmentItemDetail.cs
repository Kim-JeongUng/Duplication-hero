using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public UserItemData thisItem;
    public MyItems thisItemObject;

    protected private int sellCoinValue = 0;
    protected private int upgradeCoinValue = 0;

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
        PlaceValue.text = thisItem.value.ToString("+#.#;-#.#;0");
        PlaceSellCoin.text = sellCoinValue.ToString();
        PlaceUpgradeCoin.text = upgradeCoinValue.ToString();
        PlaceReinForceLevel.text = "LV."+thisItem.reinForceLevel.ToString();
    }
    public void SellItem()
    {
        CharacterData.instance.UserRemoveItem(thisItem.ItemIndex);
        EquipController.instance.characterDatas.Coin += sellCoinValue;
        EquipController.instance.SaveData();
        Hide();
    }
    public void UpgradeItem()
    {
        if (EquipController.instance.characterDatas.Coin >= upgradeCoinValue)
        {
            //CharacterData.instance.UserChangeItem(thisItem.ItemIndex);
            Debug.Log("Upgrade");
            EquipController.instance.characterDatas.Coin -= sellCoinValue;
            EquipController.instance.SaveData();
        }
        else
        {
            Debug.Log("µ·¾ø¾î¿ä");
        }
    }

}
