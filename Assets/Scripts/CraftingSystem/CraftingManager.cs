using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    // Variables
    public static CraftingManager instance;

    public CraftingRecipe[] recipes;
    private Dictionary<Item, CraftingRecipe> itemToRecipeDictionary = new();

    public GameObject craftingList;
    public GameObject craftingSlotPrefab;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Go through the recipes and link them to their respective items to find them easier.
        foreach (CraftingRecipe recipe in recipes)
        {
            itemToRecipeDictionary.Add(recipe.resultingItem, recipe);
        }
    }

    /// <summary>
    /// Adds items (that can be crafted) onto frames on the crafting list.
    /// </summary>
    public void PopulateCraftingList()
    {
        Dictionary<Item, int> itemList = InventoryManager.instance.GetAllItems();

        List<Item> itemsToAdd = new();

        for (int i = 0; i < recipes.Length; i++)
        {
            CraftingRecipe currentRecipe = recipes[i];
            bool canCraft = true;

            for (int j = 0; j < currentRecipe.itemsRequired.Count; j++)
            {
                if (!itemList.ContainsKey(currentRecipe.itemsRequired[j]) || itemList[currentRecipe.itemsRequired[j]] < currentRecipe.quantityRequired[j])
                {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft)
            {
                itemsToAdd.Add(currentRecipe.resultingItem);
            }
        }

        for (int i = 0; i < itemsToAdd.Count; i++)
        {
            GameObject newCraftingSlot = Instantiate(craftingSlotPrefab, craftingList.transform);
            newCraftingSlot.GetComponent<CraftingSlot>().InitialiseSlot(itemsToAdd[i]);
        }
        
        Debug.Log("Populated crafting list.");
    }

    public void UnpopulateCraftingList()
    {
        foreach (Transform child in craftingList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
