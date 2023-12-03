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
    [SerializeField] GameObject moreInfo;
    [SerializeField] TextMeshProUGUI More_InfoAbility;

    #region [Info Setting Methods]
    public void Item_InfoSetting(Sprite icon, string name, SlotTag tag, string explane, EquipStat equipStat)
    {
        Item_Icon.sprite = icon;
        Item_Name.text = name;

        MoreInfoSetActiveFalse();
        if (tag == SlotTag.None || tag == SlotTag.Potion)
        {
            abilityBtn.gameObject.SetActive(false);
            Item_ability.text = "[" + tag.ToString() + "]";
        }
        else
        {
            Item_ability.text = "[" + tag.ToString() + "]";
            abilityBtn.gameObject.SetActive(true);
            SetMoreInfoText(equipStat);
        }
        Item_Explane.text = explane;
        gameObject.SetActive(true);
    }

    public void Item_InfoNull()
    {
        Item_Icon = null;
        Item_Name.text = string.Empty;
        Item_ability.text = string.Empty;
        Item_Explane.text = string.Empty;
        abilityBtn.gameObject.SetActive(false);
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
}
