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

    //�κ��丮�� �ִ� �������� ��������Ʈ�� ī��Ʈ�� �ҷ��´�.
    //���� ���ý� �ش� �κ��丮 ���Կ� �ִ� �������� �Ǹ� �����ۿ� �ø���.
    // ������ �����ϰ� �Ǹ��Ѵ�
    // ���� : �κ��丮�� �ִ� ������ŭ �ۿ� ���ȵ��� �Ѵ�.
    // ���� ��ž��Ѵ�.
    // 
}
