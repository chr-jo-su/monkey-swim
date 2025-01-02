using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrinketSlotHolder : SlotHolder
{
    /// <summary>
    /// This adds the object to the slot if it is empty and if the item is a Trinket.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (draggableItem.storedItem.type == ItemType.Trinket)
            {
                draggableItem.parentAfterDrag = transform;

                InventoryManager.instance.EquipItem(draggableItem.storedItem);

                if (!draggableItem.equipped)
                {
                    draggableItem.equipped = true;
                }
            }
        }
    }
}
