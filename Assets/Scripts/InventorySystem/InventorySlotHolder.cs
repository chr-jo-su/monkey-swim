using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotHolder : MonoBehaviour, IDropHandler
{
    // Variables
    public Image image;
    public Color selectedSlotColor, deselectedSlotColor;

    private void Awake()
    {
        DeselectSlot();
    }

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
        image.color = selectedSlotColor;
    }

    /// <summary>
    /// Change the color of the slot to indicate that it is deselected.
    /// </summary>
    public void DeselectSlot()
    {
        image.color = deselectedSlotColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem draggableItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }
        // Can add an else statement here to check if the item can be stacked or else switched around
    }
}
