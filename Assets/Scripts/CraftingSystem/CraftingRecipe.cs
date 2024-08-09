using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Crafting recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Tooltip("The sprite that will be displayed in the inventory slot.")]
    public Sprite itemSprite;
    // This can't be done with a dictionary as it doesn't seem to let us change it in the inspector panel.
    // So instead there's 2 arrays which can be iterated through at the same time.
    [Tooltip("The list of Items that will be needed to create the resultingItem.")]
    public List<Item> itemsRequired;
    [Tooltip("The list of integers specifying how many of each Item will be needed to create the resultingItem.")]
    public List<int> quantityRequired;
    [Tooltip("The Item that's created when crafted.")]
    public Item resultingItem;

    [Tooltip("The ID of the crafting recipe. This must be unique to each crafting recipe.")]
    [ContextMenuItem("Generate a random ID", "GenerateCraftingRecipeID")]
    public int craftingRecipeID;

    /// <summary>
    /// Generates a random crafting recipe ID through an option in the Inspector when the craftingRecipeID variable is right-clicked.
    /// </summary>
    private void GenerateCraftingRecipeID()
    {
        craftingRecipeID = Random.Range(0, 999999999);
    }
}
