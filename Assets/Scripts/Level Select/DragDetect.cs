using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragDetect : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public static bool IsDrag = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDrag = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        IsDrag = false;
    }
}
