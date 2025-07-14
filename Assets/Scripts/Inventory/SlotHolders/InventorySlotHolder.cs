using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotHolder : SlotHolder
{
    // Variables
    [SerializeField] private Vector3 selectedScale;
    private Vector3 deselectedScale;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        deselectedScale = transform.localScale;
        DeselectSlot();
    }

    /// <summary>
    /// This sets the item (if any) to be draggable or not.
    /// </summary>
    /// <param name="dragOption">A boolean that specifies if the item is draggable or not.</param>
    public void SetDraggable(bool dragOption)
    {
        if (transform.childCount != 0)
        {
            if (dragOption)
            {
                transform.GetChild(0).GetComponent<InventoryItem>().draggable = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<InventoryItem>().draggable = false;
            }
        }
    }

    /// <summary>
    /// Change the scale of the slot to indicate that it is selected.
    /// </summary>
    public void SelectSlot()
    {
        transform.localScale = selectedScale;
    }

    /// <summary>
    /// Change the scale of the slot to indicate that it is deselected.
    /// </summary>
    public void DeselectSlot()
    {
        transform.localScale = deselectedScale;
    }

    /// <summary>
    /// This spawns an item in the slot if it is empty.
    /// </summary>
    /// <param name="item">The item to be added into the slot.</param>
    /// <param name="quantity">The quantity of the item to be added. Defaults to 1.</param>
    public void SpawnItem(Item item, int quantity = 1)
    {
        if (transform.childCount == 0)
        {
            GameObject newGameObjectItem = Instantiate(inventoryItemPrefab, gameObject.transform);
            InventoryItem itemInstance = newGameObjectItem.GetComponent<InventoryItem>();
            itemInstance.InitialiseItem(item, 1);
        }
    }

    /// <summary>
    /// This adds the object to the slot if it is empty.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;

            if (draggableItem.equipped)
            {
                InventoryManager.instance.UnequipItem(draggableItem.storedItem);
                draggableItem.equipped = false;
            }
        }
    }
}
