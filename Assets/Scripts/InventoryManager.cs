using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initialise the inventory slots
    public void InitialiseSlots()
    {
        GameObject inventorySlots = GameObject.Find("InventorySlots");

        // Get all the children of the inventory object
        foreach (Transform child in inventorySlots.transform)
        {
            child.gameObject.GetComponent<InventorySlotHolder>().InitialiseSlot();
        }
    }
}
