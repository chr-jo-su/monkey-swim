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
    
    [Range(0, 100)]
    [Tooltip("The amount of durability that is decreased per use. This vaule should be between 0 and 100 inclusive.")]
    public int durabilityDecreasePerUse = 0;

    [Range(0, 100)]
    [Tooltip("The damage dealt to the enemy per hit. This vaule should be between 0 and 100 inclusive.")]
    public int damagePerAttack = 0;

    [Range(0, 100)]
    [Tooltip("The boost to the player's oxygen meter. This vaule should be between 0 and 100 inclusive.")]
    public int oxygenBoost = 0;

    [Range(0, 100)]
    [Tooltip("The boost to the player's health meter. This vaule should be between 0 and 100 inclusive.")]
    public int healthBoost = 0;

    [Range(0, 25)]
    [Tooltip("The boost to the player's swim speed. This vaule should be between 0 and 25 inclusive.")]
    public int speedBoost = 0;

    [Tooltip("The type of the item.")]
    public ItemType type = ItemType.Others;
    
    [Tooltip("The ID of the item. This must be unique to each item.")]
    [ContextMenuItem("Generate a random ID", "GenerateItemID")]
    public int itemID;
    
    [Tooltip("Whether the item can be stacked or not.")]
    public bool isStackable;
    
    [Tooltip("The maximum amount of items that can be stacked together if stackable.")]
    public int maxStackSize;

    /// <summary>
    /// Generates a random item ID through an option in the Inspector when the itemID variable is right-clicked.
    /// </summary>
    private void GenerateItemID()
    {
        itemID = Random.Range(0, 999999999);
    }
}

public enum ItemType
{
    Others,         // Others is the default item type.
    Consumable,     // Consumables would usually have a durabilityDecreasePerUse of 100.
    Weapon,         // Weapons would usually have a durabilityDecreasePerUse of 1 or lower and not be stackable.
    Armour,          // Armour would usually not be stackable.
    Trinket
}
