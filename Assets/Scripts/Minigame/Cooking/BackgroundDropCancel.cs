using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundDropCancel : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// Deletes the item.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        CookingItem draggableItem = eventData.pointerDrag.GetComponent<CookingItem>();

        // Destroy the object
        Destroy(draggableItem.gameObject);
    }
}
