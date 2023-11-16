using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowMove : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _rectTransform;
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }
}
