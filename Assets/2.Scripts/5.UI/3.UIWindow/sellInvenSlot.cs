using DefineHelper;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class sellInvenSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image itemSprite;
    [SerializeField] TextMeshProUGUI Count;
    InventorySlot _Slot;
    int _num;
    public void itemSetting(InventorySlot slot, int num)
    {
        _Slot = slot;
        _num = num;
        itemSprite.sprite = _Slot.myItem.myItem.sprite;
        if (_Slot.myItem.myItem.itemTag == SlotTag.None || _Slot.myItem.myItem.itemTag == SlotTag.Potion)
        {
            Count.gameObject.SetActive(true);

            if (_Slot.myItem.currentCount <= 999)
                Count.text = _Slot.myItem.currentCount.ToString();
            else
                Count.text = "999+";
        }
    }

    public void itemRefresh()
    {
        itemSprite.sprite = _Slot.myItem.myItem.sprite;
        if (_Slot.myItem.currentCount <= 999)
            Count.text = _Slot.myItem.currentCount.ToString();
        else
            Count.text = "999+";
    }

    public void itemNull(Sprite nullSprite)
    {
        _Slot = null;
        itemSprite.sprite = nullSprite;
        Count.gameObject.SetActive(false);
    }

    public void mouseInfo(Vector2 pos)
    {
        if(_Slot != null)
            ShopWindow.Singleton.mouseInfoSetting(_Slot.myItem.myItem, pos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(_Slot != null)
                ShopWindow.Singleton.sellInvenSlotSelect(_Slot, _num);
        }
    }

    public int countNum()
    {
        return _Slot.myItem.currentCount;
    }

    //인벤토리에 있는 아이템의 스프라이트와 카운트를 불러온다.
    //슬롯 선택시 해당 인벤토리 슬롯에 있는 아이템을 판매 아이템에 올린다.
    // 수량을 선택하고 판매한다
    // 조건 : 인벤토리의 최대 수량만큼 밖에 못팔도록 한다.
    // 수량 잠궈야한다.
    // 
}
