using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    // Variables
    public static CraftingManager instance;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Adds items (that can be crafted) onto frames on the crafting list.
    /// </summary>
    public void PopulateCraftingList()
    {
        Dictionary<Item, int> itemList = InventoryManager.instance.GetAllItems();
        
        Debug.Log("Populated crafting list.");
    }

    public void UnpopulateCraftingList()
    { }
}
