using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    // Variables
    public Item storedItem;

    public Vector3 selectedScale;
    private Vector3 deselectedScale;
    
    public GameObject imagePrefab;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        deselectedScale = transform.localScale;
        DeselectSlot();
    }

    public void InitialiseSlot(Item newItem)
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
}
