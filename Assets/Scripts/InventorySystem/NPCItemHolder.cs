using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCItemHolder : MonoBehaviour
{
    // Variables
    public List<Item> items = new();

    /// <summary>
    /// Removes all items in the item holder and gives it.
    /// </summary>
    /// <returns>A list of all Item objects currently stored in the game object.</returns>
    public List<Item> DropAllItems()
    {
        List<Item> itemsToReturn = items;
        items.Clear();

        return itemsToReturn;
    }

    /// <summary>
    /// Removes a specific given Item from the item holder and gives it.
    /// </summary>
    /// <param name="itemID">The item type to drop</param>
    /// <returns>The Item object if found, else null.</returns>
    public Item DropItem(int itemID)
    {
        Item itemToDrop = null;

        foreach (Item item in items)
        {
            if (item.itemID == itemID)
            {
                itemToDrop = item;
                items.Remove(item);

                break;
            }
        }

        return itemToDrop;
    }
}
