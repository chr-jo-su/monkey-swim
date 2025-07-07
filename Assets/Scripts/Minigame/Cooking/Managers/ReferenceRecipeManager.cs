using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceRecipeManager : MonoBehaviour
{
    // Variables
    [SerializeField] private List<GameObject> recipeSlots;

    // Create a singleton instance of this game
    public static ReferenceRecipeManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSlots(List<CookingIngredient> currentIngredients)
    {
        for (int i = 0; i < recipeSlots.Count; i++)
        {
            // Add the new ingredient
            recipeSlots[i].GetComponent<ReferenceRecipeSlot>().AddIngredient(currentIngredients[i]);
        }
    }
}
