using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Variables
    public static InventoryManager instance;

    public InventorySlotHolder[] slots;
    public int hotbarSlots = 8;
    public GameObject inventoryItemPrefab;

    [HideInInspector] public int selectedSlot = -1;

    [Header("Testing variables (to be deleted later)")]
    public Item[] testItems;

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

        // Testing code; REMOVE LATER
        int added = AddItems(testItems[2], 19);
        for (int i = 0; i < 53; i++)    // Chose a weird number of items to see if the stacks are correct
        {
            int randomIndex = Random.Range(0, testItems.Length); // Get a random index
            Item randomItem = testItems[randomIndex]; // Get the random item
            added = AddItems(randomItem); // Add the item to the inventory
            if (added > 0) { break; }
        }
        DropSelectedItem(2);
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
                            IncrementItem(child);
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
                        SpawnItem(item, child);
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
    /// Spawns a new instance of the given item into the given slot
    /// </summary>
    /// <param name="item">The item object to be instantiated.</param>
    /// <param name="slot">The inventory slot object to instantiate the item in.</param>
    private void SpawnItem(Item item, InventorySlotHolder slot)
    {
        // Instantiate the game object
        GameObject newGameObjectItem = Instantiate(inventoryItemPrefab, slot.transform);

        // Add the item
        InventoryItem itemInstance = newGameObjectItem.GetComponent<InventoryItem>();
        itemInstance.InitialiseItem(item, 1);
    }

    /// <summary>
    /// Increments the item stack stored in the given slot.
    /// </summary>
    /// <param name="slot">The invenotry slot to add the item to.</param>
    private void IncrementItem(InventorySlotHolder slot)
    {
        InventoryItem itemInstance = slot.GetComponentInChildren<InventoryItem>();
        itemInstance.IncrementItem();
    }

    /// <summary>
    /// Get the selected item from the inventory.
    /// </summary>
    /// <returns>The item that's currently selected.</returns>
    public Item GetSelectedItem()
    {
        InventorySlotHolder slot = slots[selectedSlot];

        if (slot.transform.childCount != 0)
        {
            InventoryItem itemInstance = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            return itemInstance.storedItem;
        }

        return null;
    }

    /// <summary>
    /// Drops the selected item from the inventory and returns it
    /// </summary>
    /// <param name="dropQuantity">The amount of the selected item to drop. Defaults to 1.</param>
    /// <returns>The item that was dropped.</returns>
    public Item DropSelectedItem(int dropQuantity = 1)
    {
        InventorySlotHolder slot = slots[selectedSlot];

        if (slot.transform.childCount != 0)
        {
            InventoryItem itemInstance = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            itemInstance.DecrementItem(dropQuantity);

            return itemInstance.storedItem;
        }

        return null;
    }

    /// <summary>
    /// Drops all of the selected item from the inventory and returns it
    /// </summary>
    /// <returns>The item that was dropped.</returns>
    public Item DropAllSelectedItems()
    {
        InventorySlotHolder slot = slots[selectedSlot];

        if (slot.transform.childCount != 0)
        {
            InventoryItem itemInstance = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            itemInstance.DecrementItem(itemInstance.currentStackSize);

            return itemInstance.storedItem;
        }

        return null;
    }
}
