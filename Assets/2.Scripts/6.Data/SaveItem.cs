using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveItem
{
    public int itemCode;
    public int Count;

    public SaveItem()
    {

    }

    public SaveItem(InventorySlot slot)
    {
        itemCode = slot.myItem.myItem.itemCode;
        Count = slot.myItem.currentCount;
    }
}
