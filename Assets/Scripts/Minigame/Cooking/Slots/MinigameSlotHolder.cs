using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MinigameSlotHolder : MonoBehaviour
{
    // Variables
    public Image image;
    public GameObject CookingIngredientPrefab;

    /// <summary>
    /// This returns the item stored in the slot if there is one.
    /// </summary>
    /// <returns>null if there's no item stored or the CookingIngredient that's stored.</returns>
    public CookingIngredient GetStoredIngredient()
    {
        return transform.childCount > 0 ? gameObject.GetComponentInChildren<CookingItem>().storedIngredient : null;
    }

    public float GetStoredIngredientScale()
    {
        return transform.childCount > 0 ? transform.GetChild(0).localScale.x : 0f;
    }
}
