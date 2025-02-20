using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Variables
    public static InventoryManager instance;

    public InventorySlotHolder[] slots;

    public Item[] items;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Remove testing code
        // Testing code
        AddItems(items[0]);
        AddItems(items[1]);
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
    /// Removes a given quantity of the given item from the inventory.
    /// </summary>
    /// <param name="item">The given Item to be removed.</param>
    /// <param name="quantity">The quantity of the item that should be removed. Defaults to 1.</param>
    public void RemoveItems(Item item, int quantity = 1)
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

    /// <summary>
    /// Adds the boosts of the given item or trinkets to the player.
    /// </summary>
    /// <param name="item">The Item object that should be checked for boosts/trinkets.</param>
    public void EquipItem(Item item)
    {
        if (item.type == ItemType.Trinket)
        {
            // Spawn the trinket into the scene here
        }
        else if (item.type == ItemType.Armour)
        {
            // Change the player sprite to the armoured version
        }

        // Remove the oxygen boost
        OxygenBar.instance.ChangeMaxOxygen(item.oxygenBoost);

        // Remove the health boost
        PlayerHealthBar.instance.ChangeMaxHealth(item.healthBoost);

        // Remove the speed boost
        PlayerMovement.instance.ChangeMoveSpeed(item.speedBoost);
    }

    /// <summary>
    /// Removes the boosts of the given item or trinkets from the player.
    /// </summary>
    /// <param name="item">The Item object that should be checked for boosts/trinkets.</param>
    public void UnequipItem(Item item)
    {
        if (item.type == ItemType.Trinket)
        {
            // Spawn the trinket into the scene here
        }
        else if (item.type == ItemType.Armour)
        {
            // Change the player sprite back to the original
        }

        // Remove the oxygen boost
        OxygenBar.instance.ChangeMaxOxygen(-item.oxygenBoost);

        // Remove the health boost
        PlayerHealthBar.instance.ChangeMaxHealth(-item.healthBoost);

        // Remove the speed boost
        PlayerMovement.instance.ChangeMoveSpeed(-item.speedBoost);
    }
}
