using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefineHelper;
using UnityEngine.UI;

public class InventoryInfo : MonoBehaviour
{
    [Header("Info Object")]
    [SerializeField] Image Item_Icon;
    [SerializeField] TextMeshProUGUI Item_Name;
    [SerializeField] TextMeshProUGUI Item_ability;
    [SerializeField] TextMeshProUGUI Item_Explane;

    [Header("More Info")]
    [SerializeField] Button abilityBtn;
    [SerializeField] Button useBtn;
    [SerializeField] GameObject moreInfo;
    [SerializeField] TextMeshProUGUI More_InfoAbility;

    [SerializeField] InventoryItem inventoryItem;

    #region [Info Setting Methods]
    public void Item_InfoSetting(InventoryItem _invenitem)
    {
        Item_Icon.sprite = _invenitem.myItem.sprite;
        Item_Name.text = _invenitem.myItem.itemname;
        inventoryItem = _invenitem;

        MoreInfoSetActiveFalse();
        if (_invenitem.myItem.itemTag == SlotTag.None)
        {
            abilityBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(false);
            Item_ability.text = "[" + _invenitem.myItem.itemTag + "]";
        }
        else if(_invenitem.myItem.itemTag == SlotTag.Potion)
        {
            abilityBtn.gameObject.SetActive(false);
            Item_ability.text = "[" + _invenitem.myItem.itemTag + "]";
            useBtn.gameObject.SetActive(true);
        }
        else
        {
            useBtn.gameObject.SetActive(false);
            Item_ability.text = "[" + _invenitem.myItem.itemTag + "]";
            abilityBtn.gameObject.SetActive(true);
            SetMoreInfoText(_invenitem.myItem.equipstat);
        }
        Item_Explane.text = _invenitem.myItem.explane;
        gameObject.SetActive(true);
    }

    public void Item_InfoNull()
    {
        inventoryItem = null;
        Item_Name.text = string.Empty;
        Item_ability.text = string.Empty;
        Item_Explane.text = string.Empty;
        abilityBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);
        MoreInfoSetActiveFalse();
        gameObject.SetActive(false);
    }

    #endregion [Info Setting Methods]

    #region [More Info Methods]
    public void SetMoreInfoText(EquipStat equipstat)
    {
        int Textnum = 0;
        foreach(float data in equipstat)
        {
            if (data > 0)
            {
                if (!More_InfoAbility.text.Equals(string.Empty))
                    More_InfoAbility.text += "\n";
                More_InfoAbility.text += equipstat.statTextInfo(Textnum) + " : +" + data;
            }
            Textnum++;
        }
    }

    public void MoreInfo()
    {
        moreInfo.SetActive(true);

    }

    public void MoreInfoSetActiveFalse()
    {
        moreInfo.SetActive(false);
        More_InfoAbility.text = string.Empty;
    }
    #endregion [More Info Methods]

    #region [Potion Use Methods]
    public void UsePotionItem()
    {
        inventoryItem.usePotionItem();
    }

    #endregion [Potion Use Methods]
}
