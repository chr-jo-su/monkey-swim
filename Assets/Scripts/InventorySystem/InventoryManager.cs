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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Select the first slot
        ChangeSelectedSlot(0);

        // Testing code; REMOVE LATER

        bool added = AddItem(testItems[1]);
        for (int i = 0; i < 33; i++)    // Chose a weird number of items to see if the stacks are correct
        {
            int randomIndex = Random.Range(0, testItems.Length); // Get a random index
            Item randomItem = testItems[randomIndex]; // Get the random item
            added = AddItem(randomItem); // Add the item to the inventory
            if (!added) { break; }
        }
        GetSelectedItem(true);
        Debug.Log("Currently selected: " + GetSelectedItem().itemName + " (" + GetSelectedItem().type + ")");
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
    /// <returns>Returns true if the item was added successfully, otherwise returns false.</returns>
    public bool AddItem(Item item)
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
                        // Spawn the item into the slot
                        IncrementItem(child);
                        return true;
                    }
                }
            }
        }

        // Find an empty slot
        foreach (InventorySlotHolder child in slots)
        {
            if (child.transform.childCount == 0)
            {
                // Spawn the item into the slot
                SpawnItem(item, child);
                return true;
            }
        }

        return false;
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
    /// Get the selected item from the inventory and optionally use it.
    /// </summary>
    /// <param name="useItem">A boolean for whether the currently selected item should be used or not. Defaults to false.</param>
    /// <returns>The item that's currently selected.</returns>
    public Item GetSelectedItem(bool useItem = false)
    {
        InventorySlotHolder slot = slots[selectedSlot];

        if (slot.transform.childCount != 0)
        {
            InventoryItem itemInstance = slot.transform.GetChild(0).GetComponent<InventoryItem>();

            if (useItem)
            {
                itemInstance.DecrementItem();
            }

            return itemInstance.storedItem;
        }

        return null;
    }
}
