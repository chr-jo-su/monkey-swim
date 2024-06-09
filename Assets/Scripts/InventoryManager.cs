using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Initialise the inventory slots
    public void InitialiseSlots()
    {
        GameObject inventorySlots = GameObject.Find("InventorySlots");

        // Get all the children of the inventory object
        foreach (Transform child in inventorySlots.transform)
        {
            child.gameObject.GetComponent<InventorySlotHolder>().InitialiseSlot();
        }
    }

    /// <summary>
    /// Adds the items to the inventory. Stacks the item if possible, or tries to find an empty slot.
    /// </summary>
    /// <param name="item">The Item object to be added</param>
    /// <param name="count">The number of items to be added.</param>
    /// <returns>Returns the total number of items not added to the inventory. 0 means all items have been added.</returns>
    public int AddItems(Item item, int count)
    {
        GameObject inventorySlots = GameObject.Find("InventorySlots");

        // Get all the children of the inventory object
        foreach (Transform child in inventorySlots.transform)
        {
            InventorySlotHolder slot = child.gameObject.GetComponent<InventorySlotHolder>();

            // Try to add the item to the slot
            count = slot.AddItems(item, count);
            if (count == 0)
            {
                break;
            }
        }

        return count;
    }
}
