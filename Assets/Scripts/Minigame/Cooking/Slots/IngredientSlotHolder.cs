using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSlotHolder : MinigameSlotHolder
{
    /// <summary>
    /// This spawns an item in the slot if it is empty.
    /// </summary>
    public void RefillSlot(CookingIngredient ingredient)
    {
        if (transform.childCount == 0)
        {
            GameObject newGameObjectItem = Instantiate(CookingIngredientPrefab, gameObject.transform);
            CookingItem itemInstance = newGameObjectItem.GetComponent<CookingItem>();
            itemInstance.InitialiseItem(ingredient);
        }
        else if (transform.childCount > 1)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
}
