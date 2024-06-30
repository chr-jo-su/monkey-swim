using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerExitHandler
{
    // Variables
    public TMP_Text itemName, itemDescription;

    /// <summary>
    /// Sets the item info text to the given name and description
    /// </summary>
    /// <param name="name">The name of the object that's currently right-clicked.</param>
    /// <param name="description">The description of the object that's currently right-clicked.</param>
    public void SetItemInfoText(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
    }

    // When the mouse exits the object, destroy it.
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
