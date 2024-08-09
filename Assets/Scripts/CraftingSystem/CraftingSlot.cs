using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    // Variables
    public Item storedItem;

    public Vector3 selectedScale;
    private Vector3 deselectedScale;

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

    // Selects the slot when clicked. Deselects all other slots.
    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingManager.instance.DeselectAllSlots();
        SelectSlot();
        CraftingManager.instance.ShowSelectedRecipe(storedItem);
    }
}
