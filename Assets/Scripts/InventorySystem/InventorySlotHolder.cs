using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotHolder : MonoBehaviour, IDropHandler
{
    // Variables
    public Image image;

    public Vector3 selectedScale;
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

    // This adds the object to the slot if it is empty
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
