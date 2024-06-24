using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // Variables
    public Image image;
    public TMP_Text textObject;
    public Item storedItem;
    public int currentStackSize = 1;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool rightClicked = false;
    [HideInInspector] public bool draggable = true;

    void Update()
    {
        if (Input.anyKey && rightClicked)
        {
            rightClicked = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the current game object was right clicked
        if (eventData.button == PointerEventData.InputButton.Right && !rightClicked)
        {
            Debug.Log("Right clicked!");

            rightClicked = true;
        }
    }


    /// <summary>
    /// Initialise the slot with the given image and quantity
    /// </summary>
    /// <param name="newItem">The Item scriptable object to be added.</param>
    /// <param name="quantity">The amount of that item to be added.</param>
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
    /// <param name="quantity">The amount of items to add to the stack.</param>
    public void IncrementItem(int quantity = 1)
    {
        currentStackSize += quantity;
        ChangeText();
    }

    /// <summary>
    /// Remove an item from the current item stack and changes the text. If the stack is empty, the object is destroyed.
    /// </summary>
    /// <param name="quantity">The amount of items to remove from the stack.</param>
    public void DecrementItem(int quantity = 1)
    {
        currentStackSize -= quantity;
        ChangeText();

        if (currentStackSize == 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Change the text of the child text object to the given number. If the given number is 0, no text is shown.
    /// </summary>
    /// <param name="num">The number to write to the text object.</param>
    private void ChangeText()
    {
        if (currentStackSize == 0 || currentStackSize == 1)
        {
            textObject.text = "";
        }
        else
        {
            textObject.text = currentStackSize.ToString();
        }
    }

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

    public void OnDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}
