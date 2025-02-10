using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryKeyHandler : MonoBehaviour
{
    // Variables
    public static InventoryKeyHandler instance;

    private bool inventoryIsShowing;
    public GameObject inventory;
    public KeyCode inventoryKey = KeyCode.E;
    
    public GameObject hotbar;
    public int hotbarSlots = 8;

    private bool touchscreenMode;
    public GameObject craftingButton;
    public GameObject inventoryButton;

    private Vector3 showingPos = new(Screen.width / 2, Screen.height / 2, 0);
    private Vector3 hiddenPos = new(Screen.width / 2, Screen.height * 2, 0);
    private Vector3 targetPos;
    public float velocity = 5f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // If there is a touchscreen, show the inventory and crafting buttons
        touchscreenMode = Input.touchSupported;
        craftingButton.SetActive(touchscreenMode);
        inventoryButton.SetActive(touchscreenMode);

        // Start the inventory closed
        inventoryIsShowing = true;
        CloseInventory();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMenu();

        // Check for keyboard inputs
        CheckKeyboardInputs();

        if (inventory.transform.position.y - Camera.main.pixelHeight >= Camera.main.pixelHeight / 2)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
        }
    }

    /// <summary>
    /// Checks for keyboard inputs.
    /// </summary>
    private void CheckKeyboardInputs()
    {
        // Check for the inventory key to be pressed
        if (Input.GetKeyDown(inventoryKey))
        {
            if (inventoryIsShowing)
            {
                // If it's showing, hide it
                CloseInventory();
            }
            else
            {
                ShowInventory();
            }
        }
    }

    /// <summary>
    /// Closes the inventory menu. Also disables right-clicking for the item slots.
    /// </summary>
    public void CloseInventory()
    {
        if (inventoryIsShowing)
        {
            targetPos = hiddenPos;

            // Remove any tooltip menus if there are any
            try
            {
                Destroy(GameObject.Find("ItemTooltip(Clone)"));
            }
            catch (System.Exception) { }

            inventoryIsShowing = false;
        }
    }

    /// <summary>
    /// Opens the inventory menu. Also enables right-clicking for the item slots.
    /// </summary>
    private void ShowInventory()
    {
        if (!inventoryIsShowing)
        {
            // Close the crafting menu if it is open
            CraftingKeyHandler.instance.CloseCraftingMenu();

            targetPos = showingPos;

            inventoryIsShowing = true;
        }
    }

    /// <summary>
    /// Toggle the inventory menu. Useful for assigning to a button.
    /// </summary>
    public void ToggleInventory()
        {
        if (inventoryIsShowing)
        {
            CloseInventory();
        }
        else
        {
            ShowInventory();
        }
    }

    /// <summary>
    /// Animates the opening and closing of the inventory menu.
    /// </summary>
    private void AnimateMenu()
    {
        // Animate the menu moving
        inventory.transform.position = Vector3.Lerp(inventory.transform.position, targetPos, velocity * Time.unscaledDeltaTime);
    }
}
