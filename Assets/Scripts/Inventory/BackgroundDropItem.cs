using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundDropItem : MonoBehaviour, IDropHandler
{
    // Variables
    [SerializeField]
    private GameObject itemDropperPrefab;

    /// <summary>
    /// Removes the item from the player and unequips it if it's equipped.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        // Unequip the item if it's equipped
        if (draggableItem.equipped)
        {
            InventoryManager.instance.UnequipItem(draggableItem.storedItem);
        }

        // Drop the item
        //TODO: This is where the item is dropped

        // Destroy the object
        Destroy(draggableItem.gameObject);
    }
}
