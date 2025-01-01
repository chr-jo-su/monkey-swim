using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerClickHandler
{
    // Variables
    public Image image;
    public TMP_Text textObject;
    [HideInInspector] public Item storedItem;
    [HideInInspector] public int currentStackSize = 1;

    [SerializeField] private GameObject tooltipPrefab;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool draggable = true;

    private bool allowTooltip = true;
    [HideInInspector] public bool equipped = false;

    /// <summary>
    /// Checks if the item was right clicked.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the current game object was not dragged
        if (draggable)
        {
            allowTooltip = true;
            ShowTooltip();
        }
    }

    /// <summary>
    /// Don't allow the tooltip to show if the mouse exits the object.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        allowTooltip = false;
    }

    /// <summary>
    /// Initialise the slot with the given image and quantity
    /// </summary>
    /// <param name="newItem">The Item scriptable object to be added.</param>
    /// <param name="quantity">The amount of that item to be added. Defaults to 1.</param>
    public void InitialiseItem(Item newItem, int quantity = 1)
    {
        storedItem = newItem;
        image.sprite = newItem.itemSprite;

        currentStackSize = quantity;
        ChangeText();
    }

    /// <summary>
    /// Add another item to the current item stack and changes the text.
    /// </summary>
    /// <param name="quantity">The amount of items to add to the stack. Defaults to 1.</param>
    public void IncrementItem(int quantity = 1)
    {
        currentStackSize += quantity;
        ChangeText();
    }

    /// <summary>
    /// Remove an item from the current item stack and changes the text.
    /// </summary>
    /// <param name="quantity">The amount of items to remove from the stack. Defaults to 1.</param>
    public void DecrementItem(int quantity = 1)
    {
        currentStackSize -= quantity;
        ChangeText();
    }

    /// <summary>
    /// Change the text of the child text object to the given number. If the given number is 1, no text is shown.
    /// </summary>
    private void ChangeText()
    {
        if (currentStackSize == 1)
        {
            textObject.text = "";
        }
        else
        {
            textObject.text = currentStackSize.ToString();
        }
    }

    /// <summary>
    /// Change the parent to the root object and set it to not be hit by a raycast.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }

    /// <summary>
    /// Move the item to the mouse position.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// Change the parent of the item to be the new slot and allow it to be hit by a raycast.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }

    /// <summary>
    /// Shows a menu for the inventory item when hovered over. The ItemTooltip script handles destroying the object.
    /// </summary>
    public void ShowTooltip()
    {
        if (allowTooltip)
        {
            // Instantiate the tooltip
            GameObject newGameObjectItem = Instantiate(tooltipPrefab, transform.root);

            // Get the height of the tooltip
            RectTransform rectTransform = newGameObjectItem.GetComponent<RectTransform>();
            float height = rectTransform.rect.height;
            float width = rectTransform.rect.width;

            // Set the position of the tooltip
            newGameObjectItem.transform.position = Input.mousePosition;

            // Check if the tooltip is off the screen and make it always show fully
            if (Input.mousePosition.y - height < 0)
            {
                newGameObjectItem.transform.position = new Vector3(newGameObjectItem.transform.position.x, height, newGameObjectItem.transform.position.z);
            }
            if (Input.mousePosition.x + width > Screen.width)
            {
                newGameObjectItem.transform.position = new Vector3(Screen.width - width, newGameObjectItem.transform.position.y, newGameObjectItem.transform.position.z);
            }

            // Set the text info for the item
            ItemTooltip tooltip = newGameObjectItem.GetComponent<ItemTooltip>();
            tooltip.SetItemInfoText(storedItem.itemName, storedItem.itemDescription);
        }
    }
}
