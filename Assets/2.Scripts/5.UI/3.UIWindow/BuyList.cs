using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyList : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Info")]
    [SerializeField] Image _itemSprite;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemCost;

    [SerializeField] Item _item;

    public bool _nullCheck { get { if (_item == null) return true; 
                            else return false; } }

    public void SetItemInfo(Item item)
    {
        _item = item;
        _itemSprite.sprite = _item.sprite;
        _itemName.text = _item.itemname;
        _itemCost.text = _item.gold.ToString();
    }

    public void SetitemNull()
    {
        _item = null;
        this.gameObject.SetActive(false);
    }

    public void mouseInfo(Vector2 pos)
    {
        ShopWindow.Singleton.mouseInfoSetting(_item, pos);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShopWindow.Singleton.buyListSelect(_item);
        }
    }
}
