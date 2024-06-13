using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotHolder : MonoBehaviour, IDropHandler
{
    // Variables
    [HideInInspector] public int currentStackSize = 0;
    [HideInInspector] public Item storedItem;
    private bool rightClicked = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (storedItem == null)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableIcon draggableItem = dropped.GetComponent<DraggableIcon>();
            draggableItem.parentAfterDrag = transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !rightClicked)
        {
            Debug.Log("Right clicked!");
            rightClicked = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightClicked = false;
        }
    }

    /// <summary>
    /// Change the text of the child text object to the given number. If the given number is 0, no text is shown.
    /// </summary>
    /// <param name="num">The number to write to the text object.</param>
    private void ChangeText(int num)
    {
        // Change the text
        TMP_Text textObject = transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

        if (num == 0)
        {
            textObject.text = "";
        }
        else
        {
            textObject.text = num.ToString();
        }
    }

    /// <summary>
    /// TODO: Adds the given item to the inventory slot.
    /// </summary>
    /// <param name="item">The Item object to add to the slot.</param>
    /// <returns>True if the item was added, false otherwise.</returns>
    private bool AddItem(Item item)
    {
        // Check if the inventory slot is empty
        if (storedItem == null)
        {
            storedItem = item;
            currentStackSize++;

            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItem.itemIcon;
            transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            ChangeText(currentStackSize);

            return true;
        }
        else
        {
            // Check if what is currently here is the same as the item to add
            //   and if it is stackable
            //   and if the stack is not full
            if (storedItem.itemID == item.itemID && storedItem.isStackable && currentStackSize < storedItem.maxStackSize)
            {
                currentStackSize++;

                transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItem.itemIcon;
                transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);

                ChangeText(currentStackSize);

                return true;
            }
        }

        // This catches all other cases when the item hasn't been added
        return false;
    }

    /// <summary>
    /// Adds a multiple of a given item to the current inventory slot.
    /// </summary>
    /// <param name="item">The Item object to add.</param>
    /// <param name="amount">The number of objects to be added.</param>
    /// <returns>The number of items not added to the inventory slot. 0 means all items have been added.</returns>
    public int AddItems(Item item, int count)
    {
        bool added;

        while (count > 0)
        {
            added = AddItem(item);

            // If the item wasn't added, break the loop
            if (!added) { 
                break;
            }

            count--;
        }

        // Should return 0 or more.
        return count;
    }

    /// <summary>
    /// Removes the item from the inventory slot for drag and drop
    /// </summary>
    public void DragItemsAway()
    {
        storedItem = null;
        currentStackSize = 0;
    }
}
