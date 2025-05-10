using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CraftingManager : MonoBehaviour
{
    // Variables
    public static CraftingManager instance;

    public CraftingRecipe[] recipes;
    private Dictionary<Item, CraftingRecipe> itemToRecipeDictionary = new();

    public GameObject craftingList;
    public GameObject craftingSlotPrefab;
    public int itemsPerPage = 15;

    public GameObject nextPageButton;
    public GameObject previousPageButton;
    private int currentPage = 0;

    public GameObject craftingFocusSlot;
    public TMP_Text craftingFocusTitleText;
    public TMP_Text craftingFocusTypeText;
    public GameObject craftingQuantitySection;
    private Item currentSelectedItem;
    public TMP_Text craftingFocusResourcesText;
    public TMP_Text craftingFocusBoostsText;

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
    /// <param name="page">The page number that the list should be show.</param>
    public void PopulateCraftingList(int page = 0)
    {
        currentPage = page;

        Dictionary<Item, int> itemList = InventoryManager.instance.GetAllItems();

        List<Item> itemsToAdd = new();

        for (int i = 0; i < recipes.Length; i++)
        {
            itemsToAdd.Add(recipes[i].resultingItem);
        }

        for (int i = page * itemsPerPage; i < (page * itemsPerPage) + Mathf.Min(itemsToAdd.Count - (page * itemsPerPage), itemsPerPage); i++)
        {
            GameObject newCraftingSlot = Instantiate(craftingSlotPrefab, craftingList.transform);
            newCraftingSlot.GetComponent<CraftingSlot>().InitialiseSlot(itemsToAdd[i], imagePrefab);
        }

        ShowHidePagination(page, itemsToAdd);
    }

    /// <summary>
    /// Shows or hides the pagination buttons based on the current page and the items in the crafting list.
    /// </summary>
    /// <param name="page">The current page the user is on.</param>
    /// <param name="items">The list of items that can be shown in the crafting list.</param>
    private void ShowHidePagination(int page, List<Item> items)
    {
        // Show or hide the previous page button
        if (page == 0)
        {
            // Hide the previous page button
            previousPageButton.SetActive(false);
        }
        else
        {
            // Show the previous page button
            previousPageButton.SetActive(true);
        }

        // Show or hide the next page button
        if ((page + 1) * itemsPerPage < items.Count)
        {
            // Show the next page button
            nextPageButton.SetActive(true);
        }
        else
        {
            // Hide the next page button
            nextPageButton.SetActive(false);
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
    /// Shows the next page in the crafting list.
    /// </summary>
    public void ShowNextPage()
    {
        UnpopulateCraftingList();
        PopulateCraftingList(currentPage + 1);
    }

    /// <summary>
    /// Shows the previous page in the crafting list.
    /// </summary>
    public void ShowPreviousPage()
    {
        UnpopulateCraftingList();
        PopulateCraftingList(currentPage - 1);
    }

    /// <summary>
    /// Crafts the currently selected item.
    /// </summary>
    public void CraftSelectedItem()
    {
        if (currentSelectedItem != null)
        {
            CraftingRecipe selectedRecipe = itemToRecipeDictionary[currentSelectedItem];
            int selectedQuantity = craftingQuantitySection.GetComponent<CraftingQuantityHandler>().currentQuantity;

            // Remove the required items from the inventory
            for (int i = 0; i < selectedRecipe.itemsRequired.Count; i++)
            {
                InventoryManager.instance.RemoveItems(selectedRecipe.itemsRequired[i], selectedRecipe.quantityRequired[i] * selectedQuantity);
            }

            // Add the crafted item to the inventory
            InventoryManager.instance.AddItems(selectedRecipe.resultingItem, selectedQuantity);
        }

        // Recheck the crafting list
        UnpopulateCraftingList();
        PopulateCraftingList();
    }

    /// <summary>
    /// Shows the selected recipe in the crafting focus slot.
    /// </summary>
    /// <param name="selectedItem">The Item that's selected from the crafting list section.</param>
    public void ShowSelectedRecipe(Item selectedItem)
    {
        currentSelectedItem = selectedItem;

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
        craftingFocusTitleText.text = selectedItem.itemName;

        craftingFocusTypeText.text = selectedItem.type.ToString();

        // Find the recipe for the selected item
        CraftingRecipe selectedRecipe = itemToRecipeDictionary[selectedItem];

        // Set the text of the resources required for the selected item
        string resourceText = "";

        for (int i = 0; i < selectedRecipe.itemsRequired.Count; i++)
        {
            resourceText += "x" + selectedRecipe.quantityRequired[i] + " - " + selectedRecipe.itemsRequired[i].itemName + "\n";
        }

        craftingFocusResourcesText.text = resourceText;

        // Set the boosts text of the selected item
        string boostsText = "";

        if (selectedItem.oxygenBoost != 0)
        {
            boostsText += "Oxygen:             " + (selectedItem.oxygenBoost > 0 ? "+" : "") + selectedItem.oxygenBoost + "\n";
        }
        if (selectedItem.healthBoost != 0)
        {
            boostsText += "Health:              " + (selectedItem.healthBoost > 0 ? "+" : "") + selectedItem.healthBoost + "\n";
        }
        if (selectedItem.speedBoost != 0)
        {
            boostsText += "Speed:                " + (selectedItem.speedBoost > 0 ? "+" : "") + selectedItem.speedBoost + "\n";
        }
        if (selectedItem.itemDamage != 0)
        {
            boostsText += "Damage:            " + (selectedItem.itemDamage > 0 ? "+" : "") + selectedItem.itemDamage + "\n";
        }
        if (selectedItem.oxygenChange != 0)
        {
            boostsText += "Oxygen Change: " + (selectedItem.oxygenChange > 0 ? "+" : "") + selectedItem.oxygenChange + "\n";
        }

        craftingFocusBoostsText.text = boostsText;

        // Find out the maximum number of items that can be crafted
        int maxCraftable = InventoryManager.instance.GetMaximumCapacity(selectedItem);

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

        craftingQuantitySection.GetComponent<CraftingQuantityHandler>().ResetQuantitySection(maxCraftable == 0 ? 0 : 1);
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

        craftingFocusTitleText.text = "Item to craft";
        craftingFocusResourcesText.text = "";
        craftingFocusTypeText.text = "";
        craftingFocusBoostsText.text = "";

        craftingQuantitySection.GetComponent<CraftingQuantityHandler>().ResetQuantitySection(0);
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
