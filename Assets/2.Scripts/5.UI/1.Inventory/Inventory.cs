using DefineHelper;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;
    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;
    [SerializeField] InventoryInfo inventoryInfo;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Btn")]
    [SerializeField] Button _closeBtn;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    [Header("PlayerEquipmentInfo")]
    [SerializeField] PlayerEquipmentInfo _playerEquipemntInfo;

    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
        _closeBtn.onClick.AddListener(delegate { Close_Inventory(); });
    }

    void Update()
    {
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item) //아이템 옮기기
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem); // 여기가 아이템 장착 장소
        }
        else if(carriedItem == null && item != null && item.activeSlot.myTag == SlotTag.Weapon)
        {
            unEquipWeapon();
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);

        if (carriedItem != null && item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != SlotTag.Potion)
        { 
                EquipEquipment(carriedItem.activeSlot.myTag, carriedItem, true);
        } // 여기가 아이템 해제 장소
    }

    public void SetItemInfo(InventoryItem item)
    {
        if (carriedItem != null)
            return;

        if (item.myItem.itemTag == SlotTag.None || item.myItem.itemTag == SlotTag.Potion)
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, string.Empty, item.myItem.explane);
        else if (item.myItem.itemTag == SlotTag.Weapon)
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, item.myItem.Att.ToString(), item.myItem.explane);
        else
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, item.myItem.Def.ToString(), item.myItem.explane);
    }
    public void SetItemInfoNull()
    {
        inventoryInfo.Item_InfoNull();
    }

    public void EquipEquipment(SlotTag tag, InventoryItem item, bool _EquipCheck = false)
    {
        switch (tag)
        {
            case SlotTag.Head:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Chest:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Legs:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Feet:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Gloves:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Shoulders:
                EquipAmor(item, _EquipCheck);
                break;
            case SlotTag.Weapon:
                EquipWeapon(item, _EquipCheck);
                break;
        }
    }

    public void EquipAmor(InventoryItem item, bool _EquipCheck)
    {
        var obj = _playerEquipemntInfo._amorObjectDic[item.myItem.equipmentStr];
        SetEquipment(obj);

        _playerEquipemntInfo.EquipStatData(item, _EquipCheck);

        if (item.activeSlot.myTag == SlotTag.Legs)//바지 속옷
            _playerEquipemntInfo.UnderwearSetActive(!obj.activeSelf);
    }

    public void EquipWeapon(InventoryItem item, bool _EquipCheck)
    {
        if (!_EquipCheck)
        { 
            var obj = _playerEquipemntInfo._amorObjectDic[item.myItem.equipmentStr];
            _playerEquipemntInfo._player.WeaponEquip(obj);
        }

        _playerEquipemntInfo.EquipStatData(item, _EquipCheck);
    }

    public  void unEquipWeapon()
    {
        _playerEquipemntInfo._player.WeaponEquip(null);
    }

    public void SetEquipment(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        // 장비 효과 player에 주기
    }

    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item;
        int _itemNum = -1;
        bool _createCheck = false;
        if (_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem != null
                 && _item.sprite == inventorySlots[i].myItem.myItem.sprite
                  && inventorySlots[i].myItem.currentCount < inventorySlots[i].myItem.myItem.MaxNumber)
            {
                inventorySlots[i].myItem.increaseCount(1);
                _createCheck = true;
                break;
            }
            // Check if the slot is empty
            else if (inventorySlots[i].myItem == null && _itemNum == -1)
                _itemNum = i;
        }

        if(!_createCheck)
            Instantiate(itemPrefab, inventorySlots[_itemNum].transform).Initialize(_item, inventorySlots[_itemNum]);
    }

    public void SpawnInventoryItem_test(Item item, int _number)
    {
        Item _item = item;
        int _itemNum = -1;
        bool _createCheck = false;
        if (_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem != null
                 && _item.sprite == inventorySlots[i].myItem.myItem.sprite
                  && inventorySlots[i].myItem.currentCount < inventorySlots[i].myItem.myItem.MaxNumber)
            {
                inventorySlots[i].myItem.increaseCount(1);
                _createCheck = true;
                break;
            }
            // Check if the slot is empty
            else if (inventorySlots[i].myItem == null && _itemNum == -1)
                _itemNum = i;
        }

        if (!_createCheck)
            Instantiate(itemPrefab, inventorySlots[_itemNum].transform).Initialize(_item, inventorySlots[_itemNum]);
    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }

    void Close_Inventory()
    {
        SetItemInfoNull();
        transform.parent.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
