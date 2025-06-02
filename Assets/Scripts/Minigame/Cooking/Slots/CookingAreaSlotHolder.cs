using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookingAreaSlotHolder : MinigameSlotHolder, IDropHandler
{
    /// <summary>
    /// This adds the object to the slot if it is empty.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            CookingItem draggableItem = eventData.pointerDrag.GetComponent<CookingItem>();
            draggableItem.parentAfterDrag = transform;
            draggableItem.isOnPrepArea = false;
        }
    }

    public void HighlightSlot()
    {
        GetComponent<Outline>().enabled = true;
    }

    public void UnhighlightSlot()
    {
        GetComponent<Outline>().enabled = false;
    }
}
