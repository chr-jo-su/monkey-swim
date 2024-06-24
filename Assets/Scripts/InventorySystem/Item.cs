using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    // Item descriptors
    [Header("Visible to players")]
    [Tooltip("The name of the item that will be displayed in the inventory.")]
    public string itemName;
    [TextArea]
    [Tooltip("The description of the item that will be displayed in the inventory.")]
    public string itemDescription;
    [Tooltip("The sprite that will be displayed in the inventory slot.")]
    public Sprite itemSprite;
    [Range(0, 100)]
    [Tooltip("The amount of durability that the item has. This value should be between 0 and 100 inclusive.")]
    public int durability = 100;

    [Header("Not visible to players")]
    [Range(1, 100)]
    [Tooltip("The amount of durability that is decreased per use. This vaule should be between 1 and 100 inclusive.")]
    public int durabilityDecreasePerUse;
    [Tooltip("The type of the item.")]
    public ItemType type;
    [Tooltip("The ID of the item. This must be unique to each item.")]
    [ContextMenuItem("Generate a random ID", "GenerateItemID")]
    public int itemID;
    [Tooltip("Whether the item can be stacked or not.")]
    public bool isStackable;
    [Tooltip("The maximum amount of items that can be stacked together if stackable.")]
    public int maxStackSize;

    private void GenerateItemID()
    {
        itemID = Random.Range(0, 999999999);
    }
}

public enum ItemType
{
    // Others is the default item type, don't remove it
    Others,
    Consumable,
    Weapon,
    Armour
}
