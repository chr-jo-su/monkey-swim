using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item storedItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");

        parentBeforeDrag = transform.parent;

        storedItem = parentBeforeDrag.GetComponent<InventorySlotHolder>().storedItem;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        parentAfterDrag.GetChild(0).SetParent(parentBeforeDrag);

        parentAfterDrag.GetComponent<InventorySlotHolder>().AddItems(storedItem, parentBeforeDrag.GetComponent<InventorySlotHolder>().currentStackSize);

        parentBeforeDrag.GetComponent<InventorySlotHolder>().DragItemsAway();
    }
}
