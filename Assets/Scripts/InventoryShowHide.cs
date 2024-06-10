using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryShowHide : MonoBehaviour
{
    // Variables
    private bool isShowing = false;
    public KeyCode inventoryKey = KeyCode.E;
    public GameObject inventory;
    public Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        // Used for the close button
        Button btn = closeButton.GetComponent<Button>();
        btn.onClick.AddListener(CloseInventory);

        // Initialise the inventory slots
        inventory.GetComponent<InventoryManager>().InitialiseSlots();

        // Set the inventory to be hidden at the start
        inventory.SetActive(false);

        Debug.Log("Inventory key check is active.");

        //inventory.GetComponent<InventoryManager>().AddItems(transform.root.Find("Items").GetChild(0).GetComponent<Item>(), 1);
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

    // Closes the inventory
    void CloseInventory()
    {
        AnimateScaleChange(inventory, inventory.transform.localScale, new Vector2(0, 0));

        inventory.SetActive(false);
        isShowing = false;

        Debug.Log("Inventory closed");
    }

    // Opens the inventory
    void ShowInventory()
    {
        inventory.SetActive(true);

        AnimateScaleChange(inventory, inventory.transform.localScale, new Vector2(1, 1));

        isShowing = true;

        Debug.Log("Inventory opened");
    }

    //TODO: Animates the scale of the given gameObject from startScale to endScale
    void AnimateScaleChange(GameObject gameObject, Vector2 startScale, Vector2 endScale) { }
}
