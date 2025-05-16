using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Objects/Minigame/Cooking Ingredient")]
public class CookingIngredient : ScriptableObject
{
    [Tooltip("The ID of the ingredient. This must be unique to each ingredient.")]
    [ContextMenuItem("Generate a random ID", "GenerateIngredientID")]
    public int ingredientID = 0;

    [Tooltip("The name of the ingredient that will be displayed in the tooltip when clicked.")]
    public string ingredientName;

    [Tooltip("The sprite that will be displayed in the slot.")]
    public Sprite itemSprite;

    [Tooltip("The type of ingredient. This determines what the user needs to do to convert it.")]
    public IngredientType ingredientType = IngredientType.None;

    [Tooltip("The amount of time it takes to get the ingredient to convert into it's next form. This value is only used if the ingredient type is 'Timed'.")]
    [Range(0f, 30f)]
    public float timeToConvert;

    [Tooltip("The number of clicks it takes to get the ingredient to convert into it's next form. This value is only used if the ingredient type is 'Clickable'.")]
    [Range(0, 30)]
    public int clicksToConvert;

    [Tooltip("The ingredients that the current ingredient converts to once it's finished with clicking or waiting. This value will be null if it's the final form or it can be multiple if there's more than one thing that's made from it.")]
    public List<CookingIngredient> nextForms;

    [Tooltip("The sound effects that will be played when the ingredient is clicked (can be used for timed or clickable ingredients).")]
    public List<AudioClip> inProgressSFXClips;

    [Tooltip("The sound effects that will be played when the ingredient is clicked (can be used for timed or clickable ingredients).")]
    public List<AudioClip> completedSFXClips;

    /// <summary>
    /// Generates a random item ID through an option in the Inspector when the ingredientID variable is right-clicked.
    /// This should be unique to each ingredient, not to each instance of every ingredient.
    /// </summary>
    private void GenerateIngredientID()
    {
        ingredientID = Random.Range(0, 99999999);
    }

    public AudioClip GetRandomInProgressSFXClip()
    {
        return inProgressSFXClips.Count > 0 ? inProgressSFXClips[Random.Range(0, inProgressSFXClips.Count)] : null;
    }

    public AudioClip GetRandomCompletedSFXClip()
    {
        return completedSFXClips.Count > 0 ? completedSFXClips[Random.Range(0, completedSFXClips.Count)] : null;
    }
}

public enum IngredientType
{
    None,       // This is the default type. No ingredient should be this type and will not work if set to this type.
    Clickable,  // This is the type for ingredients that should be clicked by the user to chop, mash, dice, etc.
    Timed,      // This is the type for ingredients that convert after a set amount of time.
}
