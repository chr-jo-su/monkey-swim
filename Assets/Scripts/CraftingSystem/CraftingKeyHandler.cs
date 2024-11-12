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

    [HideInInspector] private Vector3 openPos = new(Screen.width / 2, Screen.height / 2, 0);
    [HideInInspector] private Vector3 closePos = new(Screen.width / 2, Screen.height * 2, 0);
    [HideInInspector] private Vector3 targetPos;
    private float velocity = 5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMenu();

        // Check for the crafting menu key to be pressed
        if (Input.GetKeyDown(craftingMenuKey))
        {
            if (isShowing)
            {
                // If it is showing, hide it
                CloseCraftingMenu();
            }
            else
            {
                ShowCraftingMenu();
            }
        }

        if (craftingMenu.transform.position.y - Camera.main.pixelHeight >= Camera.main.pixelHeight / 2)
        {
            craftingMenu.SetActive(false);
        }
        else
        {
            craftingMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Closes the crafting menu.
    /// </summary>
    public void CloseCraftingMenu()
    {
        if (isShowing)
        {
            targetPos = closePos;

            // Remove any tooltip menus if there are any
            try
            {
                Destroy(GameObject.Find("ItemTooltip(Clone)"));
            }
            catch (System.Exception) { }

            // Remove all the items from the crafting list
            CraftingManager.instance.UnpopulateCraftingList();

            isShowing = false;
        }
    }

    /// <summary>
    /// Opens the crafting menu.
    /// </summary>
    private void ShowCraftingMenu()
    {
        if (!isShowing)
        {
            // Close the inventory if it is open
            InventoryKeyHandler.instance.CloseInventory();

            // Add all items that can be crafted to the crafting list
            CraftingManager.instance.PopulateCraftingList();

            targetPos = openPos;

            isShowing = true;
        }
    }

    /// <summary>
    /// Toggle the crafting menu. Useful for assigning to a button.
    /// </summary>
    public void ToggleCraftingMenu()
    {
        if (isShowing)
        {
            CloseCraftingMenu();
        }
        else
        {
            ShowCraftingMenu();
        }
    }

    /// <summary>
    /// Animates the opening and closing of the crafting menu.
    /// </summary>
    private void AnimateMenu()
    {
        // Animate the menu moving
        craftingMenu.transform.position = Vector3.Lerp(craftingMenu.transform.position, targetPos, velocity * Time.unscaledDeltaTime);
    }
}
