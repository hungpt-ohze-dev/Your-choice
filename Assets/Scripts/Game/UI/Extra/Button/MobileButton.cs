using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Action OnButtonDown;
    public Action OnButtonUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonUp?.Invoke();
    }
}
