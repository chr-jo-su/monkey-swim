using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerExitHandler
{
    // Variables
    public TMP_Text itemName, itemDescription;
    public GameObject equipButton;

    /// <summary>
    /// Shows or hides the equip button based on whether the item is armour.
    /// </summary>
    /// <param name="isEquipable">A boolean value that specifies if the item attached is armour or not.</param>
    public void ChangeWindowType(bool isEquipable)
    {
        equipButton.SetActive(isEquipable);
    }

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

    /// <summary>
    /// When the mouse exits the object, destroy the tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
