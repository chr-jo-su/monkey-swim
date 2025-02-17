using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmourSlotHolder : SlotHolder
{
    /// <summary>
    /// This adds the object to the slot if it is empty and is Armour.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (draggableItem.storedItem.type == ItemType.Armour)
            {
                draggableItem.parentAfterDrag = transform;

                if (!draggableItem.equipped)
                {
                    InventoryManager.instance.EquipItem(draggableItem.storedItem);
                    draggableItem.equipped = true;
                }
            }
        }
    }
}
