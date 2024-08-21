using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryKeyHandler : MonoBehaviour
{
    // Variables
    public static InventoryKeyHandler instance;

    [HideInInspector] private bool inventoryIsShowing;
    public GameObject inventory;
    public KeyCode inventoryKey = KeyCode.E;
    public GameObject hotbar;
    public int hotbarSlots = 8;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryIsShowing = true;

        // Start the inventory closed
        CloseInventory();
    }

    // Update is called once per frame
    void Update()
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
            inventory.SetActive(false);
            inventoryIsShowing = false;

            // Disable dragging for the hotbar items
            InventoryManager.instance.GetComponent<InventoryManager>().SetDraggable(false);

            // Remove any tooltip menus if there are any
            try
            {
                Destroy(GameObject.Find("ItemTooltip(Clone)"));
            }
            catch (System.Exception) { }

            InventoryManager.instance.ReselectPreviousSlot();
        }
    }

    /// <summary>
    /// Opens the inventory menu. Also enables dragging and right-clicking for the hotbar items.
    /// </summary>
    public void ShowInventory()
    {
        if (!inventoryIsShowing)
        {
            // Close the crafting menu if it is open
            CraftingKeyHandler.instance.CloseCraftingMenu();

            inventory.SetActive(true);
            inventoryIsShowing = true;

            // Enable dragging for the hotbar items
            InventoryManager.instance.GetComponent<InventoryManager>().SetDraggable(true);

            InventoryManager.instance.DeselectAllSlots();
        }
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
