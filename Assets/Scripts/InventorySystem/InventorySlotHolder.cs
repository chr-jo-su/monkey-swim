using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotHolder : MonoBehaviour, IDropHandler
{
    // Variables
    public Image image;
    public GameObject inventoryItemPrefab;

    [SerializeField] private Vector3 selectedScale;
    private Vector3 deselectedScale;

    [SerializeField] private Vector2 equippedSize = new(5, -5);
    private Vector2 unequippedSize = new(0, 0);

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        deselectedScale = transform.localScale;
        DeselectSlot();
        UnequipSlot();
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
    /// Change the color of the slot to indicate that it is selected.
    /// </summary>
    public void SelectSlot()
    {
        transform.localScale = selectedScale;
    }

    /// <summary>
    /// Change the color of the slot to indicate that it is deselected.
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
    /// This adds the object to the slot if it is empty
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }
        // Can add an else statement here to check if the item can be stacked or else switched around
    }

    /// <summary>
    /// Equips the item currently stored in the slot and toggles visual changes when equipping.
    /// Also unequips any previous items.
    /// </summary>
    public void EquipItem()
    {
        InventoryManager.instance.UnequipAllItems();

        // Change equip status to true
        GetComponentInChildren<InventoryItem>().equipped = true;

        // Get stored item in InventoryItem
        Item item = GetComponentInChildren<InventoryItem>().storedItem;

        InventoryManager.instance.EquipItem(item);
        EquipSlot();
    }

    /// <summary>
    /// Unequips the item currently stored in the slot and also toggles visual changes.
    /// </summary>
    public void UnequipItem()
    {
        Item storedItem = GetStoredItem();

        if (storedItem != null && storedItem.type == ItemType.Armour)
        {
            if (GetComponentInChildren<InventoryItem>().equipped == true)
            {
                // Change equip status to false
                GetComponentInChildren<InventoryItem>().equipped = false;

                // Get stored item in InventoryItem
                Item item = GetComponentInChildren<InventoryItem>().storedItem;

                InventoryManager.instance.UnequipItem(item);
                UnequipSlot();
            }
        }
    }

    /// <summary>
    /// Shows the visual cue that the slot is equipped.
    /// </summary>
    public void EquipSlot()
    {
        gameObject.GetComponent<Outline>().effectDistance = equippedSize;
    }

    /// <summary>
    /// Hides the visual cue that the slot is equipped.
    /// </summary>
    public void UnequipSlot()
    {
        gameObject.GetComponent<Outline>().effectDistance = unequippedSize;
    }
}
