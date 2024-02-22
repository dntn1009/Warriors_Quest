using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineHelper;


public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }

    public SlotTag myTag;

    public HotbarSlot hotbarSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            if (Inventory.carriedItem == null) return;
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return;

            SetItem(Inventory.carriedItem); // (장착과정) 1. Slot에 아이템이 있지 않은 경우 여기는 무조건 장비 슬롯에 item이 없는 경우이다.
        }
    }

    public void SetItem(InventoryItem item, bool nullCheck = true)
    {
        Inventory.carriedItem = null;

        // Reset old slot
        item.activeSlot.myItem = null;

        // Set current slot
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;

        if (myTag != SlotTag.None)
        {
            if (myTag == SlotTag.Potion)
                hotbarSlot.SettingHotbar(item.myItem.sprite, item.CountStr);
            else
                Inventory.Singleton.equipEquipMent(item, nullCheck); // 여기가 장착 장소임. 장비 슬롯에 아이템의 유무에 따라 다르게 작동함.
        }
    }
    public void SellitemDestroy()
    {
        myItem.canvasGroup.blocksRaycasts = false;
        Destroy(myItem.gameObject);
        myItem = null;

    }

    #region [Hotbar & UserPotion Methods]

    public void UsePotionItem()
    {
        if (myItem != null)
        {
            Inventory.Singleton.UsePotionItemEffect(myItem);
            hotbarSlot.SettingHotbar(myItem.myItem.sprite, myItem.CountStr);
        }
    }

    public void HotborSlotSettingHotbar()
    {
        hotbarSlot.SettingHotbar(myItem.myItem.sprite, myItem.CountStr);
    }

    public void HotbarActiveFalse()
    {
        hotbarSlot.SettingFalse();
    }
    #endregion [Hotbar & UserPotion Methods]

    
}
