using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Variables
    public static InventoryManager instance;

    public InventorySlotHolder[] slots;
    public int hotbarSlots = 8;

    [HideInInspector] public int selectedSlot = -1;

    public Item[] items;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Select the first slot
        ChangeSelectedSlot(0);

        // Testing code. This should allow you to craft one diamond pickaxe
        AddItems(items[0], 73);
        AddItems(items[1], 3);
        AddItems(items[2], 11);
        RemoveItems(items[0], 40);
    }

    /// <summary>
    /// Set the draggable option for all hotbar slots to the given value.
    /// </summary>
    /// <param name="dragOption">A boolean value that specifies if the slots should be draggable or not.</param>
    public void SetDraggable(bool dragOption)
    {
        for (int i = 0; i < hotbarSlots; i++)
        {
            slots[i].SetDraggable(dragOption);
        }
    }

    /// <summary>
    /// Checks if the inventory has the given item in the given quantity.
    /// </summary>
    /// <param name="item">The item to check for.</param>
    /// <param name="quantity">The quantity of the item to check for. Defaults to 1.</param>
    /// <returns></returns>
    public bool CheckForItems(Item item, int quantity = 1)
    {
        int totalItems = 0;

        // Go through each slot and see if the item is there
        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount != 0)
            {
                InventoryItem inventoryItem = child.transform.GetChild(0).GetComponent<InventoryItem>();

                if (inventoryItem.storedItem.itemID == item.itemID)
                {
                    totalItems += inventoryItem.currentStackSize;
                }
            }
        }

        // See if it's enough
        if (totalItems >= quantity)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns all the items and quantities currently in the inventory.
    /// </summary>
    /// <returns>A dictionary containing all the items (as keys) and their quantities (as values).</returns>
    public Dictionary<Item, int> GetAllItems()
    {
        Dictionary<Item, int> items = new();

        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount != 0)
            {
                InventoryItem inventoryItem = child.transform.GetChild(0).GetComponent<InventoryItem>();

                if (items.ContainsKey(inventoryItem.storedItem))
                {
                    items[inventoryItem.storedItem] += inventoryItem.currentStackSize;
                }
                else
                {
                    items.Add(inventoryItem.storedItem, inventoryItem.currentStackSize);
                }
            }
        }

        return items;
    }

    /// <summary>
    /// Returns the total number of Items that can fit into the remaining slots.
    /// </summary>
    /// <param name="item">The Item to check for spaces.</param>
    /// <returns>An integer specifying the total number of Item that can fit into the unfilled slots.</returns>
    public int GetMaximumCapacity(Item item)
    {
        int total = 0;

        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount == 1)
            {
                if (child.transform.GetChild(0).GetComponent<InventoryItem>().storedItem == item)
                {
                    total += item.maxStackSize - child.transform.GetChild(0).GetComponent<InventoryItem>().currentStackSize;
                }
            }
        }

        total += GetTotalEmptySlots() * item.maxStackSize;

        return total;
    }

    /// <summary>
    /// Returns the total number of empty slots
    /// </summary>
    /// <returns>An integer specifying the total number of completely empty slots in the player's inventory.</returns>
    public int GetTotalEmptySlots()
    {
        int total = 0;

        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount == 0)
            {
                total++;
            }
        }

        return total;
    }

    /// <summary>
    /// Changes the selected slot to the given slot position. This doesn't deselect the previous slot.
    /// </summary>
    /// <param name="slotPosition">An integer that specifies the slot position to select. Works for the hotbar only.</param>
    public void ChangeSelectedSlot(int slotPosition)
    {
        selectedSlot = slotPosition % hotbarSlots;
        UpdateSelectedSlot();
    }

    /// <summary>
    /// Changes the selected slot to the given slot position. This also deselects the previous slot.
    /// </summary>
    /// <param name="slotPosition">An integer that specifies the slot position to select. Works for the hotbar only.</param>
    /// <param name="previousSlotPosition">An integer that specifies the slot position to deselect. Works for the hotbar only.</param>
    public void ChangeSelectedSlot(int slotPosition, int previousSlotPosition)
    {
        selectedSlot = slotPosition % hotbarSlots;
        UpdateSelectedSlot(previousSlotPosition);
    }

    /// <summary>
    /// Change the selected slot based on the given direction. This also deselects the previous slot.
    /// </summary>
    /// <param name="direction">A string that specifies the direction as either "left" or "right".</param>
    public void ChangeSelectedSlot(string direction)
    {
        // Save the previous slot to deselect it
        int previousSlot = selectedSlot;

        if (direction == "right")
        {
            selectedSlot++;
            selectedSlot %= hotbarSlots;
        }
        else if (direction == "left")
        {
            selectedSlot--;

            if (selectedSlot < 0)
            {
                selectedSlot = hotbarSlots - 1;
            }
        }

        UpdateSelectedSlot(previousSlot);
    }

    /// <summary>
    /// Deselects all the slots in the hotbar. Useful when the inventory is open and no slots should be selected.
    /// </summary>
    public void DeselectAllSlots()
    {
        for (int i = 0; i < hotbarSlots; i++)
        {
            slots[i].DeselectSlot();
        }
    }

    /// <summary>
    /// Reselects the previously selected slot. Useful when the inventory is closed and needs to show a selected slot again.
    /// </summary>
    public void ReselectPreviousSlot()
    {
        UpdateSelectedSlot();
    }

    /// <summary>
    /// Selects the new slot and deselects the given slot if needed.
    /// </summary>
    /// <param name="previousSlot">The integer value of the slot to deselect. Defaults to -1 for no deselecting. Works for the hotbar only.</param>
    private void UpdateSelectedSlot(int previousSlot = -1)
    {
        if (previousSlot != -1)
        {
            slots[previousSlot % hotbarSlots].DeselectSlot();
        }

        slots[selectedSlot].SelectSlot();
    }

    /// <summary>
    /// Adds the given item to the inventory. Tries to find an empty slot, or stacks the item if possible.
    /// </summary>
    /// <param name="item">The Item object to be added</param>
    /// <param name="quantity">The amount of the item to be added. Defaults to 1.</param>
    /// <returns>Returns 0 if all the items were added successfully, or the number of items not added.</returns>
    public int AddItems(Item item, int quantity = 1)
    {
        int prevQuantity = quantity;

        while (quantity > 0)
        {
            // Find a slot with the given item and available space if it's stackable
            if (item.isStackable)
            {
                foreach (InventorySlotHolder child in slots)
                {
                    if (child.transform.childCount != 0)
                    {
                        InventoryItem inventoryItem = child.transform.GetChild(0).GetComponent<InventoryItem>();

                        if (inventoryItem.storedItem.itemID == item.itemID &&
                            inventoryItem.currentStackSize < item.maxStackSize)
                        {
                            // Add the item into the slot
                            child.IncrementItem();
                            quantity--;

                            break;
                        }
                    }
                }
            }

            if (prevQuantity == quantity)
            {
                // Find an empty slot
                foreach (InventorySlotHolder child in slots)
                {
                    if (child.transform.childCount == 0)
                    {
                        // Spawn the item into the slot
                        child.SpawnItem(item, 1);
                        quantity--;

                        break;
                    }
                }
            }

            // If no items were added, break the loop
            if (prevQuantity == quantity)
            {
                break;
            }
            prevQuantity = quantity;
        }

        return quantity;
    }

    /// <summary>
    /// Get the selected item from the inventory.
    /// </summary>
    /// <returns>The item that's currently selected.</returns>
    public Item GetSelectedItem()
    {
        InventorySlotHolder slot = slots[selectedSlot];

        Item storedItem = null;

        if (slot.transform.childCount != 0)
        {
            InventoryItem itemInstance = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            storedItem = itemInstance.storedItem;
        }

        return storedItem;
    }

    /// <summary>
    /// Drops the selected item from the inventory and returns it.
    /// </summary>
    /// <param name="dropQuantity">The amount of the selected item to drop. Defaults to 1.</param>
    /// <returns>The item that was dropped.</returns>
    public Item DropSelectedItem(int dropQuantity = 1)
    {
        Item storedItem = null;
        InventorySlotHolder slot = slots[selectedSlot];

        storedItem = slot.GetStoredItem();
        slot.DecrementItem(dropQuantity);

        return storedItem;
    }

    /// <summary>
    /// Drops all of the selected item from the inventory and returns it.
    /// </summary>
    /// <returns>The item that was dropped.</returns>
    public Item DropAllSelectedItems()
    {
        Item storedItem = null;
        InventorySlotHolder slot = slots[selectedSlot];

        storedItem = slot.GetStoredItem();
        slot.DecrementItem(slot.GetCurrentStackSize());

        return storedItem;
    }

    /// <summary>
    /// Removes a given quantity of the given item from the inventory and returns it.
    /// </summary>
    /// <param name="item">The given Item to be removed.</param>
    /// <param name="quantity">The quantity of the item that should be removed. Defaults to 1.</param>
    /// <returns>The Item that was removed.</returns>
    public Item RemoveItems(Item item, int quantity = 1)
    {
        int total = 0;

        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount != 0)
            {
                if (child.GetStoredItem().itemID == item.itemID)
                {
                    if (child.GetCurrentStackSize() >= quantity)
                    {
                        child.DecrementItem(quantity);
                        break;
                    }
                    else
                    {
                        total = child.GetCurrentStackSize();
                        child.DecrementItem(quantity);
                        quantity -= total;
                    }
                }
            }
        }

        return item;
    }

    /// <summary>
    /// Sorts the inventory items and stacks items as much as possible.
    /// </summary>
    public void SortInventory()
    {
        // Sort the items
        Dictionary<Item, int> items = new();

        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount != 0)
            {
                InventoryItem inventoryItem = child.transform.GetChild(0).GetComponent<InventoryItem>();

                if (items.ContainsKey(inventoryItem.storedItem))
                {
                    items[inventoryItem.storedItem] += inventoryItem.currentStackSize;
                }
                else
                {
                    items[inventoryItem.storedItem] = inventoryItem.currentStackSize;
                }
            }
        }

        // Clear the previous items
        foreach (Item item in items.Keys)
        {
            RemoveItems(item, items[item]);
        }

        // Add the items back
        foreach (Item item in items.Keys)
        {
            AddItems(item, items[item]);
        }
    }
}
