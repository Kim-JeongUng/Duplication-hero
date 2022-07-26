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
        if (thisItem.isEquip) // �������̸� ���� �� ����
        {
            EquipItemButton();
        }
        DataManager.instance.UserRemoveItem(thisItemObject.ItemIndex);
        EquipController.instance.characterDatas.Coin += sellCoinValue;
        DataManager.instance.Save(EquipController.instance.characterDatas);
        EquipController.instance.SaveData();
        Destroy(thisItemObject);
        SceneManager.LoadScene("EquipmentScene");
    }
    public void UpgradeItemButton()
    {
        if (EquipController.instance.characterDatas.Coin >= upgradeCoinValue)
        {
            Debug.Log("Upgrade");
            EquipController.instance.characterDatas.Coin -= upgradeCoinValue;

            DataManager.instance.Save(EquipController.instance.characterDatas);
            if (thisItem.isEquip) // �������̸� ���� �� ���׷��̵� �� ������
            {
                EquipItemButton();
                thisItem.value *= 1.1f; //�ɷ�ġ 10%���
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
        }
        else
        {
            Debug.Log("�������");
        }

        thisItemObject.OpenEquipDeatilCanvas();
    }
    public void EquipItemButton()
    {
        thisItemObject.Equip();
        thisItemObject.EquipCheck();
        PlaceEquipText.text = thisItem.isEquip ? "UnEquip" : "Equip";
    }

}
