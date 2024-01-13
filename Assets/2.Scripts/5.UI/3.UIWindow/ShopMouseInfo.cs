using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefineHelper;

public class ShopMouseInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemExplane;
    [SerializeField] TextMeshProUGUI itemAbility;

    public void mouseInfoSetting(Item item)
    {
        itemName.text = item.itemname;
        itemExplane.text = item.explane;
        
        if(item.itemTag == SlotTag.None || item.itemTag == SlotTag.Potion)
        {
            itemAbility.text = string.Empty;
        }
        else
        {
            itemAbility.text = string.Empty;

            int Textnum = 0;
            foreach (float data in item.equipstat)
            {
                if (data > 0)
                {
                    if (!itemAbility.text.Equals(string.Empty))
                        itemAbility.text += "\n";
                    itemAbility.text += item.equipstat.statTextInfo(Textnum) + " : +" + data;
                }
                Textnum++;
            }
        }
    }

}
