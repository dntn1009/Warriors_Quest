using DefineHelper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [Header("Item Slots & Gold")]
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;
    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;
    [SerializeField] InventoryInfo inventoryInfo;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] TextMeshProUGUI goldText;
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
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(null, 100); });
        _closeBtn.onClick.AddListener(delegate { Close_Inventory(); });
        SetGoldInfo();
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
        else if (carriedItem == null && item != null && item.activeSlot.myTag == SlotTag.Weapon)
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

    public void unEquipWeapon()
    {
        _playerEquipemntInfo._player.WeaponEquip(null);
    }

    public void SetEquipment(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        // 장비 효과 player에 주기
    }

    public void SetGoldInfo()
    {
       goldText.text = _playerEquipemntInfo._player._stat.GOLD.ToString();
    }

    public void SpawnInventoryItem(Item item, int _number = 1)
    {
        Item _item = item;

        if (_item == null)
        { _item = PickRandomItem(); }

        if (_item.itemTag != SlotTag.None && _item.itemTag != SlotTag.Potion)
            _number = 1;


        if (_item.itemTag == SlotTag.None || _item.itemTag == SlotTag.Potion)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].myItem != null
                     && _item == inventorySlots[i].myItem.myItem)
                {
                    if (inventorySlots[i].myItem.currentCount == inventorySlots[i].myItem.myItem.MaxNumber)
                        continue;
                    else if (inventorySlots[i].myItem.currentCount + _number > inventorySlots[i].myItem.myItem.MaxNumber)
                    {
                        _number += inventorySlots[i].myItem.currentCount;
                        _number -= inventorySlots[i].myItem.myItem.MaxNumber;
                        inventorySlots[i].myItem.increaseMax();
                    }
                    else
                    {
                        inventorySlots[i].myItem.increaseCount(_number);
                        _number = 0;
                        break;
                    }
                }
            }
            if (_number > 0)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].myItem == null)
                    {
                        if (_number > _item.MaxNumber)
                        {
                            Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i], _item.MaxNumber);
                            _number -= _item.MaxNumber;
                        }
                        else
                        {
                            Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i], _number);
                            _number = 0;
                        }
                    }
                    if (_number <= 0)
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].myItem == null)
                {
                    Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i], _number);
                    break;
                }
            }
        }
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
