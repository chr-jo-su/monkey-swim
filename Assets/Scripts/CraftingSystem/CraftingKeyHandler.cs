using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingKeyHandler : MonoBehaviour
{
    // Variables
    [HideInInspector] public bool isShowing = true;
    public GameObject craftingMenu;
    public KeyCode craftingMenuKey = KeyCode.Q;

    // Start is called before the first frame update
    void Start()
    {
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
        craftingMenu.SetActive(false);
        isShowing = false;
    }

    /// <summary>
    /// Opens the crafting menu.
    /// </summary>
    public void ShowCraftingMenu()
    {
        craftingMenu.SetActive(true);
        isShowing = true;
    }
}
