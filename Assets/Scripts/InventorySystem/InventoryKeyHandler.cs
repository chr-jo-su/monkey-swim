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
        // If there is a touchscreen, show the inventory and crafting buttons
        touchscreenMode = Input.touchSupported;
        craftingButton.SetActive(touchscreenMode);
        inventoryButton.SetActive(touchscreenMode);
        Debug.Log(touchscreenMode);

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

        // Only check keys for the hotbar if the inventory is closed
        if (!inventoryIsShowing)
        {
            // Check for the scroll wheel to be scrolled
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                // Scroll up
                InventoryManager.instance.ChangeSelectedSlot("right");
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                // Scroll down
                InventoryManager.instance.ChangeSelectedSlot("left");
            }

            // Check for the number keys to be pressed
            for (int i = 1; i <= hotbarSlots; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    InventoryManager.instance.ChangeSelectedSlot(i - 1, InventoryManager.instance.selectedSlot);
                }
            }
        }
    }

    /// <summary>
    /// Closes the inventory menu. Also disables dragging and right-clicking for the hotbar items.
    /// </summary>
    public void CloseInventory()
    {
        if (inventoryIsShowing)
        {
            targetPos = closePos;

            // Disable dragging for the hotbar items
            InventoryManager.instance.GetComponent<InventoryManager>().SetDraggable(false);

            // Remove any tooltip menus if there are any
            try
            {
                Destroy(GameObject.Find("ItemTooltip(Clone)"));
            }
            catch (System.Exception) { }

            InventoryManager.instance.ReselectPreviousSlot();

            inventoryIsShowing = false;
        }
    }

    /// <summary>
    /// Opens the inventory menu. Also enables dragging and right-clicking for the hotbar items.
    /// </summary>
    private void ShowInventory()
    {
        if (!inventoryIsShowing)
        {
            // Close the crafting menu if it is open
            CraftingKeyHandler.instance.CloseCraftingMenu();

            // Enable dragging for the hotbar items
            InventoryManager.instance.GetComponent<InventoryManager>().SetDraggable(true);

            InventoryManager.instance.DeselectAllSlots();

            targetPos = openPos;

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

    /// <summary>
    /// Hides the hotbar game object.
    /// </summary>
    public void HideHotbar()
    {
        hotbar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the hotbar game object.
    /// </summary>
    public void ShowHotbar()
    {
        hotbar.gameObject.SetActive(true);
    }

    /// <summary>
    /// Toggles the hotbar game object.
    /// </summary>
    public void ToggleHotbar()
    {
        hotbar.gameObject.SetActive(!hotbar.gameObject.activeSelf);
    }
}
