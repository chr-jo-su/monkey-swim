using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    // Variables
    public static CraftingManager instance;

    public CraftingRecipe[] recipes;
    private Dictionary<Item, CraftingRecipe> itemToRecipeDictionary = new();

    public GameObject craftingList;
    public GameObject craftingSlotPrefab;

    public GameObject craftingFocusSlot;
    public TMP_Text craftingFocusText;
    public GameObject craftingQuantitySection;

    public GameObject imagePrefab;

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
            newCraftingSlot.GetComponent<CraftingSlot>().InitialiseSlot(itemsToAdd[i], imagePrefab);
        }
    }

    /// <summary>
    /// Removes all the items from the crafting list. Also resets the crafting focus slot.
    /// </summary>
    public void UnpopulateCraftingList()
    {
        foreach (Transform child in craftingList.transform)
        {
            Destroy(child.gameObject);
        }

        ResetCraftingFocus();
    }

    /// <summary>
    /// Removes all the items that were craftable, and rechecks then readds them back to the list.
    /// </summary>
    public void RepopulateCraftingList()
    {
        UnpopulateCraftingList();
        PopulateCraftingList();
    }

    /// <summary>
    /// Shows the selected recipe in the crafting focus slot.
    /// </summary>
    /// <param name="selectedItem">The Item that's selected from the crafting list section.</param>
    public void ShowSelectedRecipe(Item selectedItem)
    {
        // Remove the previous item from the crafting focus slot if any
        if (craftingFocusSlot.transform.childCount > 0)
        {
            Destroy(craftingFocusSlot.transform.GetChild(0).gameObject);
        }

        // Add the new item to the crafting focus slot
        GameObject newImage = Instantiate(imagePrefab, craftingFocusSlot.transform);
        newImage.GetComponent<Image>().sprite = selectedItem.itemSprite;
        newImage.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -90);

        // Set the text to the name of the item
        craftingFocusText.text = selectedItem.itemName;

        // Find the recipe for the selected item
        CraftingRecipe selectedRecipe = itemToRecipeDictionary[selectedItem];

        // Find out the maximum that can be crafted
        int maxCraftable = InventoryManager.instance.GetTotalEmptySlots(selectedItem);

        Dictionary<Item, int> itemList = InventoryManager.instance.GetAllItems();

        for (int i = 0; i < selectedRecipe.itemsRequired.Count; i++)
        {
            if (itemList.ContainsKey(selectedRecipe.itemsRequired[i]))
            {
                int maxCraftableFromItem = itemList[selectedRecipe.itemsRequired[i]] / selectedRecipe.quantityRequired[i];
                if (maxCraftableFromItem < maxCraftable)
                {
                    maxCraftable = maxCraftableFromItem;
                }
            }
            else
            {
                maxCraftable = 0;
                break;
            }
        }

        craftingQuantitySection.GetComponent<CraftingQuantityHandler>().ResetQuantitySection();
        craftingQuantitySection.GetComponent<CraftingQuantityHandler>().SetMaxQuantity(maxCraftable);
    }

    /// <summary>
    /// Reset the crafting focus slot and quantity section.
    /// </summary>
    private void ResetCraftingFocus()
    {
        if (craftingFocusSlot.transform.childCount > 0)
        {
            Destroy(craftingFocusSlot.transform.GetChild(0).gameObject);
        }

        craftingFocusText.text = "Select an item to craft";

        craftingQuantitySection.GetComponent<CraftingQuantityHandler>().ResetQuantitySection();
    }

    /// <summary>
    /// Deselects all slots in the crafting list.
    /// </summary>
    public void DeselectAllSlots()
    {
        for (int i = 0; i < craftingList.transform.childCount; i++)
        {
            CraftingSlot slot = craftingList.transform.GetChild(i).GetComponent<CraftingSlot>();
            slot.DeselectSlot();
        }
    }
}
