using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackgroundDropItem : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        // Drop item and remove it from the inventory
        Debug.Log("Dropped " + draggableItem.currentStackSize + " " + draggableItem.storedItem.itemName);

        // Most likely give the dropped item to some other game object here before destroying it

        Destroy(draggableItem.gameObject);
    }
}
