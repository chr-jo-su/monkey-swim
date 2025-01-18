using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotHolder : MonoBehaviour, IDropHandler
{
    // Variables
    public Image image;
    public GameObject inventoryItemPrefab;

    /// <summary>
    /// Increments the item in the slot.
    /// </summary>
    /// <param name="quantity">The quantity to increase the stack by. Defaults to 1.</param>
    public void IncrementItem(int quantity = 1)
    {
        if (transform.childCount != 0)
        {
            InventoryItem itemInstance = gameObject.GetComponentInChildren<InventoryItem>();
            itemInstance.IncrementItem(quantity);
        }
    }

    /// <summary>
    /// Decrements the item in the slot. Also removes the item if the stack is empty.
    /// </summary>
    /// <param name="quantity">The quantity to remove from the stack. Defaults to 1.</param>
    public void DecrementItem(int quantity = 1)
    {
        if (transform.childCount != 0)
        {
            InventoryItem itemInstance = gameObject.GetComponentInChildren<InventoryItem>();
            itemInstance.DecrementItem(quantity);

            // Remove item if there's no more left
            if (itemInstance.currentStackSize <= 0)
            {
                // Get a pointer to the item
                GameObject child = gameObject.transform.GetChild(0).gameObject;
                // Detach the item from the slot
                gameObject.transform.DetachChildren();
                // Destroy the item using the pointer
                Destroy(child);
            }
        }
    }

    /// <summary>
    /// This returns the item stored in the slot if there is one.
    /// </summary>
    /// <returns>null if there's no item stored or the Item that's stored.</returns>
    public Item GetStoredItem()
    {
        if (transform.childCount == 0)
        {
            return null;
        }
        else
        {
            return gameObject.GetComponentInChildren<InventoryItem>().storedItem;
        }
    }

    /// <summary>
    /// This returns the current stack size of the item in the slot.
    /// </summary>
    /// <returns>An integer specifying the current stack size. Returns 0 if there's no item in the slot.</returns>
    public int GetCurrentStackSize()
    {
        if (transform.childCount != 0)
        {
            return gameObject.GetComponentInChildren<InventoryItem>().currentStackSize;
        }
        return 0;
    }

    /// <summary>
    /// This adds the object to the slot if it is empty.
    /// </summary>
    /// <param name="eventData"></param>
    public abstract void OnDrop(PointerEventData eventData);
}
