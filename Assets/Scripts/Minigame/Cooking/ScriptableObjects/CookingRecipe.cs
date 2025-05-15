using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/Minigame/Cooking Recipe")]
public class CookingRecipe : ScriptableObject
{
    [Tooltip("The name of the recipe that will be displayed on the screen when it's being made.")]
    public string recipeName;

    [Tooltip("The list of all the ingredients required (in order).")]
    public List<CookingIngredient> ingredients;

    [Tooltip("The amount of time given to the player to make the current dish.")]
    [Range(0, 2 * 60)]
    public float maxTime;
}
