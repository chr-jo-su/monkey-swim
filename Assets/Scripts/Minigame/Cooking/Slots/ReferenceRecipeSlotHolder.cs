using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceRecipeSlot : MinigameSlotHolder
{
    public void AddIngredient(CookingIngredient ingredient)
    {
        RemoveIngredient();

        GameObject newGameObjectItem = Instantiate(CookingIngredientPrefab, gameObject.transform);
        CookingItem itemInstance = newGameObjectItem.GetComponent<CookingItem>();
        itemInstance.InitialiseItem(ingredient, false);
    }

    public void RemoveIngredient()
    {
        if (transform.childCount != 0)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            gameObject.transform.DetachChildren();
            Destroy(child);
        }
    }
}
