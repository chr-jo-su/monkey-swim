using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Variables
    public Item storedItem;

    public Vector3 selectedScale;
    private Vector3 deselectedScale;

    public GameObject tooltipPrefab;
    public float tooltipDelay = 0.75f;
    private bool allowTooltip = true;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        deselectedScale = transform.localScale;
        DeselectSlot();
    }

    /// <summary>
    /// Initialises the slot with the given item and image prefab.
    /// </summary>
    /// <param name="newItem">The item to be added to the slot.</param>
    /// <param name="imagePrefab">The prefab of the image to be shown in the slot.</param>
    public void InitialiseSlot(Item newItem, GameObject imagePrefab)
    {
        storedItem = newItem;

        // Add an image game object to the slot
        GameObject newImage = Instantiate(imagePrefab, transform);

        // Set the sprite of the image to the sprite of the item
        newImage.GetComponent<Image>().sprite = newItem.itemSprite;
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
    /// Selects the slot when clicked. Deselects all other slots.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingManager.instance.DeselectAllSlots();
        SelectSlot();
        CraftingManager.instance.ShowSelectedRecipe(storedItem);
    }

    /// <summary>
    /// Checks if the mouse was hovering over the object for a certain amount of time.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        allowTooltip = true;
        Invoke("ShowTooltip", tooltipDelay);
    }

    /// <summary>
    /// Don't try to show the tooltip if the mouse exits the object.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        allowTooltip = false;
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

            // Set the text for the item info
            newGameObjectItem.GetComponent<ItemTooltip>().SetItemInfoText(storedItem.itemName, storedItem.itemDescription);
        }
    }
}
