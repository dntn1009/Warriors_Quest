using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] BuyList buylist;
    [SerializeField] sellInvenSlot sellslot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buylist != null)
        {
            var pos = new Vector3(transform.position.x + 90f, transform.position.y + 45f, 0);
            buylist.mouseInfo(pos);
        }
        else
        {
            var pos = new Vector3(transform.position.x + 45f, transform.position.y + 45f, 0);
            sellslot.mouseInfo(pos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopWindow.Singleton.mouseInfoFalse();
    }
}
