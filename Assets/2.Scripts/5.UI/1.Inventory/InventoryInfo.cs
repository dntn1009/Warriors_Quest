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

    public void Item_InfoSetting(Sprite icon, string name, SlotTag tag, string ability, string explane)
    {
        Item_Icon.sprite = icon;
        Item_Name.text = name;
        
        if(tag == SlotTag.None || tag == SlotTag.Potion)
            Item_ability.text = "[" + tag.ToString() + "]\n" + ability;
        else if(tag == SlotTag.Weapon)
            Item_ability.text = "[" +  tag.ToString() + "]\n" + "공격력 : +" + ability;
        else
            Item_ability.text = "[" + tag.ToString() + "]\n" + "방어력 : +" + ability;

        Item_Explane.text = explane;
        gameObject.SetActive(true);
    }

    public void Item_InfoNull()
    {
        Item_Icon = null;
        Item_Name.text = string.Empty;
        Item_ability.text = string.Empty;
        Item_Explane.text = string.Empty;
        gameObject.SetActive(false);
    }
}
