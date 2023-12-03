using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowMove : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _rectTransform;
    Vector2 _initPos;

    public void InitSetPos()
    {
        _initPos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        Vector2 move = mousePos - _initPos;
        _rectTransform.anchoredPosition += move;
        _initPos = mousePos;
    }
}