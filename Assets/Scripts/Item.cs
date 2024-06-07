using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Variables
    public string itemName;
    public int itemID;
    [TextArea]
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject itemModel;
    public bool isStackable;
    public int maxStackSize;
}
