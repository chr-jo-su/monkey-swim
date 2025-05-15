using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingAreaManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cookingAreaSlots;
    [SerializeField] private PrepAreaSlotHolder ingredientPrepAreaSlot;

    // Create a singleton of this class
    public static CookingAreaManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ClearSlots()
    {
        foreach (GameObject slot in cookingAreaSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    public List<CookingIngredient> GetCurrentItems()
    {
        List<CookingIngredient> currentItems = new();

        foreach (GameObject slot in cookingAreaSlots)
        {
            if (slot.transform.childCount == 1)
            {
                currentItems.Add(slot.transform.GetComponentInChildren<CookingItem>().storedIngredient);
            }
            else
            {
                currentItems.Add(null);
            }
        }

        return currentItems;
    }

    public void ConvertCurrentItem()
    {
        List<CookingIngredient> nextForms = ingredientPrepAreaSlot.GetStoredIngredient().nextForms;
        float scale = ingredientPrepAreaSlot.GetStoredIngredientScale();

        // Remove it and add the next form items to the prep area
        ingredientPrepAreaSlot.RemoveIngredients();

        foreach (CookingIngredient ingredient in nextForms)
        {
            ingredientPrepAreaSlot.AddIngredient(ingredient, scale);
        }
    }

    public void UnhighlightAllSlots()
    {
        foreach (GameObject slot in cookingAreaSlots)
        {
            slot.GetComponent<CookingAreaSlotHolder>().UnhighlightSlot();
        }
    }

    public void HighlightSlot(int slot)
    {
        cookingAreaSlots[slot].GetComponent<CookingAreaSlotHolder>().HighlightSlot();
    }
}
