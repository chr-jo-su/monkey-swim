using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Objects/Trinket")]
public class Trinket : Item {
    [Range(0, 100)]
    [Tooltip("The boost to the player's oxygen meter. This vaule should be between 0 and 100 inclusive.")]
    public int oxygenBoost = 0;

    [Range(0, 100)]
    [Tooltip("The boost to the player's health meter. This vaule should be between 0 and 100 inclusive.")]
    public int healthBoost = 0;

    [Range(0, 25)]
    [Tooltip("The boost to the player's swim speed. This vaule should be between 0 and 25 inclusive.")]
    public int speedBoost = 0;
}
