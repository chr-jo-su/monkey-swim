using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingKeyHandler : MonoBehaviour
{
    // Variables
    public static CraftingKeyHandler instance;

    private bool isShowing;
    public GameObject craftingMenu;
    [SerializeField] private KeyCode craftingMenuKey = KeyCode.Q;

    private Vector3 showingPos = new(0, 0, 0);
    private Vector3 hiddenPos;
    private Vector3 targetPos;
    [SerializeField] private float velocity = 5f;

    /// <summary>
    /// Creates a singleton instance of the CraftingKeyHandler.
    /// </summary>
    private void Awake()
    {
        instance = this;
        hiddenPos = new(0, Screen.height * 1.5f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        isShowing = true;

        // Start the crafting closed
        CloseCraftingMenu();

        // Scale the background to screen size
        craftingMenu.transform.GetChild(0).localScale = new Vector3(Screen.width / 1920f, Screen.height / 1080f, 1);
    }

    // Update is called once per frame
    private void Update()
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
        targetPos = hiddenPos;

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

            targetPos = showingPos;

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
        craftingMenu.transform.localPosition = Vector3.Lerp(craftingMenu.transform.localPosition, targetPos, velocity * Time.unscaledDeltaTime);
    }

    public bool IsShowing()
    {
        return isShowing;
    }
}
