using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingQuantityHandler : MonoBehaviour
{
    // Variables
    private int maxQuantity;
    [HideInInspector] public int currentQuantity;

    public TMP_Text quantityText;

    // Start is called before the first frame update
    void Start()
    {
        ResetQuantitySection();
    }

    /// <summary>
    /// Sets the maximum allowed quantity to the given value.
    /// </summary>
    /// <param name="maxQuantity">An integer specifying the maximum allowed quantity.</param>
    public void SetMaxQuantity(int maxQuantity)
    {
        this.maxQuantity = maxQuantity;
    }

    /// <summary>
    /// Increments the quantity by one and changes the text.
    /// </summary>
    public void IncrementQuantity()
    {
        currentQuantity++;
        currentQuantity = Mathf.Clamp(currentQuantity, 0, maxQuantity);

        SetText();
    }

    /// <summary>
    /// Decrements the quantity text by one and changes the text.
    /// </summary>
    public void DecrementQuantity()
    {
        currentQuantity--;
        currentQuantity = Mathf.Clamp(currentQuantity, 0, maxQuantity);

        SetText();
    }

    /// <summary>
    /// Resets the variables and text to the original values.
    /// </summary>
    public void ResetQuantitySection()
    {
        maxQuantity = 0;
        currentQuantity = 0;

        SetText();
    }

    /// <summary>
    /// Sets the text object to the current quantity.
    /// </summary>
    private void SetText()
    {
       quantityText.text = currentQuantity.ToString();
    }
}
