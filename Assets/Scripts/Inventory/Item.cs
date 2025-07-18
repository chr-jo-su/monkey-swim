using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/Item")]
public class Item : ScriptableObject
{
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
    [Tooltip("The amount of durability that is decreased per use. This value should be between 0 and 100 inclusive.")]
    public int durabilityDecreasePerUse = 0;

    [Range(0, 100)]
    [Tooltip("The damage dealt to the enemy per hit. This value should be between 0 and 100 inclusive.")]
    public int damagePerAttack = 0;

    [Tooltip("The type of the item.")]
    public ItemType type = ItemType.Others;

    [Tooltip("The ID of the item. This must be unique to each item.")]
    [ContextMenuItem("Generate a random ID", "GenerateItemID")]
    public int itemID = 0;

    [Tooltip("Whether the item can be stacked or not.")]
    public bool isStackable = false;

    [Tooltip("The maximum amount of items that can be stacked together if stackable. isStackable must be true for this value to be used.")]
    public int maxStackSize = 1;

    [Header("Change these values if the item is a trinket")]

    [Range(-50, 100)]
    [Tooltip("The boost to the player's oxygen meter. This value should be between -50 and 100 inclusive.")]
    public int oxygenBoost = 0;

    [Range(-50, 100)]
    [Tooltip("The boost to the player's health meter. This value should be between -50 and 100 inclusive.")]
    public int healthBoost = 0;

    [Range(-15, 15)]
    [Tooltip("The boost to the player's swim speed. This value should be between -15 and 15 inclusive.")]
    public int speedBoost = 0;

    [Range(-15, 15)]
    [Tooltip("The boost to the item's damage. This value should be between -15 and 15 inclusive.")]
    public int itemDamage = 0;
    
    [Range(-10f, 0.9f)]
    [Tooltip("The change to the player's rate of change of oxygen. This value should be between -10 and 0.9 inclusive.")]
    public float oxygenChange = 0;

    [Range(-15, 15)]
    [Tooltip("The change to the player's acceleration. This value should be between 0 and 15 inclusive.")]
    public int accelerationChange = 0;

    /// <summary>
    /// Generates a random item ID through an option in the Inspector when the itemID variable is right-clicked.
    /// This should be unique to each item, not to each instance of every item.
    /// </summary>
    private void GenerateItemID()
    {
        itemID = Random.Range(0, 99999999);
    }
}

public enum ItemType
{
    Others,         // Others is the default item type. Most, if not all, items should not be this type.
    Weapon,         // Weapons have a durabilityDecreasePerUse of 1 or higher and are not stackable.
    Trinket         // Trinkets are not stackable.
}
