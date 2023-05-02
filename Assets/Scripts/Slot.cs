using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
   [SerializeField] private int score;

  public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
      
        //if cursor is dragging object
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "Object")
        {
            //Debug.Log(eventData.pointerDrag);
            //snapping dragged object into slot
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            score++;
        }
    }
}
