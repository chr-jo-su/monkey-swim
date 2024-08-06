using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingKeyHandler : MonoBehaviour
{
    // Variables
    public static CraftingKeyHandler instance;

    [HideInInspector] private bool isShowing;
    public GameObject craftingMenu;
    public KeyCode craftingMenuKey = KeyCode.Q;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isShowing = true;

        // Start the crafting closed
        CloseCraftingMenu();

        Debug.Log("Crafting system is active.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the crafting menu key to be pressed
        if (Input.GetKeyDown(craftingMenuKey))
        {
            if (craftingMenu.activeSelf)
            {
                // If it is showing, hide it
                CloseCraftingMenu();
            }
            else
            {
                ShowCraftingMenu();
            }
        }
    }

    /// <summary>
    /// Closes the crafting menu.
    /// </summary>
    public void CloseCraftingMenu()
    {
        if (isShowing)
        {
            // Animate the crafting menu closing
            AnimateMenu(false);

            // Remove all the items from the crafting list
            CraftingManager.instance.UnpopulateCraftingList();

            craftingMenu.SetActive(false);

            isShowing = false;
        }
    }

    /// <summary>
    /// Opens the crafting menu.
    /// </summary>
    public void ShowCraftingMenu()
    {
        if (!isShowing)
        {
            // Close the inventory if it is open
            InventoryKeyHandler.instance.CloseInventory();

            craftingMenu.SetActive(true);

            // Add all items that can be crafted to the crafting list
            CraftingManager.instance.PopulateCraftingList();

            // Animate the crafting menu opening
            AnimateMenu(true);

            isShowing = true;
        }
    }

    /// <summary>
    /// Animates the opening and closing of the crafting menu.
    /// </summary>
    public void AnimateMenu(bool open)
    {
        if (open) { }
        else { }
    }
}
