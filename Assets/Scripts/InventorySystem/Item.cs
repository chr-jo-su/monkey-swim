using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    // Item descriptors
    [Header("Visible to players")]
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemSprite;

    [Header("Not visible to players")]
    public ItemType type;
    public int itemID;
    public bool isStackable;
    public int maxStackSize;
}

public enum ItemType
{
    // Others is the default item type, don't remove it
    Others,
    Consumable,
    Weapon,
    Armour
}
