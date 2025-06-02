using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientRefill : MonoBehaviour
{
    // Variables
    [SerializeField] private List<GameObject> ingredientSlots;
    [SerializeField] private List<CookingIngredient> ingredients;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            ingredientSlots[i].GetComponent<IngredientSlotHolder>().RefillSlot(ingredients[i]);
        }
    }
}
