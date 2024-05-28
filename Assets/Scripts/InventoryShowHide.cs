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
        btn.onClick.AddListener(closeInventory);

        // Set the inventory to be hidden at the start
        inventory.SetActive(false);

        Debug.Log("Inventory key check is active.");
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
                closeInventory();
            }
            else
            {
                showInventory();
            }
        }
    }

    // Closes the inventory
    void closeInventory()
    {
        inventory.SetActive(false);
        isShowing = false;

        Debug.Log("Inventory closed");
    }

    // Opens the inventory
    void showInventory()
    {
        inventory.SetActive(true);
        isShowing = true;

        Debug.Log("Inventory opened");
    }

    //TODO: Animates the scale of the given gameObject from startScale to endScale
    void animateScaleChange(GameObject gameObject, Vector2 startScale, Vector2 endScale) { }
}
