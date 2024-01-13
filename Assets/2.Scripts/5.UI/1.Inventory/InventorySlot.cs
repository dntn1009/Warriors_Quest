using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DefineHelper;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }

    public SlotTag myTag;

    [SerializeField] HotbarSlot HotbarSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            if (Inventory.carriedItem == null) return;
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return;

            SetItem(Inventory.carriedItem); // (��������) 1. Slot�� �������� ���� ���� ��� ����� ������ ��� ���Կ� item�� ���� ����̴�.
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
                HotbarSlot.SettingHotbar(item.myItem.sprite, item.CountStr);
            else
                Inventory.Singleton.equipEquipMent(item, nullCheck); // ���Ⱑ ���� �����. ��� ���Կ� �������� ������ ���� �ٸ��� �۵���.
        }
    }

    public void UsePotionItem()
    {
        if (myItem != null)
        {
            Inventory.Singleton.UsePotionItemEffect(myItem);
            HotbarSlot.SettingHotbar(myItem.myItem.sprite, myItem.CountStr);
        }
    }

    public void HotborSlotSettingHotbar()
    {
        HotbarSlot.SettingHotbar(myItem.myItem.sprite, myItem.CountStr);
    }

    public void HotbarActiveFalse()
    {
        HotbarSlot.SettingFalse();
    }

    public void SellitemDestroy()
    {
        myItem.canvasGroup.blocksRaycasts = false;
        Destroy(myItem.gameObject);
        myItem = null;
        
    }
}
