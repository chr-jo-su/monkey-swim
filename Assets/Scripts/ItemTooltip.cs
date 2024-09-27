using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ItemTooltip : MonoBehaviour, IPointerExitHandler
{
    // Variables
    public TMP_Text itemName, itemDescription;
    public GameObject equipButton;

    /// <summary>
    /// Shows or hides the equip button based on whether the item is armour.
    /// Also sets the button onClick function.
    /// </summary>
    /// <param name="isEquipable">A boolean value that specifies if the item attached is armour or not.</param>
    /// <param name="slot">The slot that the tooltip is linked to. Defaults to null. Used for the equip button.</param>
    /// <param name="equip">A boolean value that specifies if the button should equip or unequip the item. Defaults to true.</param>
    public void ChangeWindowType(bool isEquipable, InventorySlotHolder slot = null, bool equip = true)
    {
        equipButton.SetActive(isEquipable);

        if (slot != null)
        {
            if (equip)
            {
                equipButton.GetComponent<Button>().onClick.AddListener(slot.EquipItem);
                equipButton.GetComponent<Button>().onClick.AddListener(DestroySelf);
            }
            else
            {
                equipButton.GetComponent<Button>().onClick.AddListener(slot.UnequipItem);
                equipButton.GetComponent<Button>().onClick.AddListener(DestroySelf);
            }
        }
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
    /// Destroys the current instance of the game object.
    /// </summary>
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// When the mouse exits the object, destroy the tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData = null)
    {
        Destroy(gameObject);
    }
}
