using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Sprite sprite;
    public Image image;
    [HideInInspector] public Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item storedItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;

        storedItem = parentBeforeDrag.GetComponent<InventorySlotHolder>().storedItem;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        parentAfterDrag.GetChild(0).SetParent(parentBeforeDrag);

        parentBeforeDrag.GetComponent<InventorySlotHolder>().AddItems(storedItem, parentAfterDrag.GetComponent<InventorySlotHolder>().currentStackSize);

        parentAfterDrag.GetComponent<InventorySlotHolder>().DragItemsAway();
    }
}
