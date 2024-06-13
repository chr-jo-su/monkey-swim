using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryKeyHandler : MonoBehaviour
{
    // Variables
    [HideInInspector] public bool isShowing = false;
    public KeyCode inventoryKey = KeyCode.E;
    private GameObject inventory;
    public Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        inventory = transform.GetChild(0).gameObject;

        // Set the inventory to be hidden at the start
        inventory.SetActive(false);

        //TEMP: Adds an item to the inventory to test that it works
        //Item testItem = new();
        //inventory.GetComponent<InventoryManager>().AddItems(testItem, 1);
        //transform.root.GetChild(transform.root.childCount - 1).SetParent(null);

        Debug.Log("Inventory system is working.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the given key being pressed
        if (Input.GetKeyDown(inventoryKey))
        {
            if (inventory.activeSelf)
            {
                // If it is showing, hide it
                CloseInventory();
            }
            else
            {
                ShowInventory();
            }
        }
    }

    /// <summary>
    /// Closes the inventory menu.
    /// </summary>
    public void CloseInventory()
    {
        AnimateScaleChange(inventory, inventory.transform.localScale, new Vector2(0, 0));

        inventory.SetActive(false);
        isShowing = false;
    }

    /// <summary>
    /// Opens the inventory menu.
    /// </summary>
    public void ShowInventory()
    {
        inventory.SetActive(true);

        AnimateScaleChange(inventory, inventory.transform.localScale, new Vector2(1, 1));

        isShowing = true;
    }

    //TODO: Animates the scale of the given gameObject from startScale to endScale
    void AnimateScaleChange(GameObject gameObject, Vector2 startScale, Vector2 endScale) { }
}
