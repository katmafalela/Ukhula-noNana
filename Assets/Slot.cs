using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
  public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
      
        //if cursor is dragging object
        if (eventData.pointerDrag != null)
        {
            //snapping dragged object into slot
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
