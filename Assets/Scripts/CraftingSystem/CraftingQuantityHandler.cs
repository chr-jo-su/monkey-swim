using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingQuantityHandler : MonoBehaviour {
    // Variables
    private int maxQuantity;
    [HideInInspector] public int currentQuantity;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject incrementButton;
    [SerializeField] private GameObject decrementButton;

    // Start is called before the first frame update
    void Start() {
        ResetQuantitySection(0);
    }

    /// <summary>
    /// Sets the maximum allowed quantity to the given value.
    /// </summary>
    /// <param name="maxQuantity">An integer specifying the maximum allowed quantity.</param>
    public void SetMaxQuantity(int maxQuantity) {
        this.maxQuantity = maxQuantity;
        CheckToHide();
    }

    /// <summary>
    /// Increments the quantity by one and changes the text.
    /// </summary>
    public void IncrementQuantity() {
        currentQuantity++;
        currentQuantity = Mathf.Clamp(currentQuantity, maxQuantity == 0 ? 0 : 1, maxQuantity);

        SetText();
        CheckToHide();
    }

    /// <summary>
    /// Decrements the quantity text by one and changes the text.
    /// </summary>
    public void DecrementQuantity() {
        currentQuantity--;
        currentQuantity = Mathf.Clamp(currentQuantity, maxQuantity == 0? 0 : 1, maxQuantity);

        SetText();
        CheckToHide();
    }

    /// <summary>
    /// Resets the variables and text to the original values.
    /// </summary>
    public void ResetQuantitySection(int startAmount) {
        maxQuantity = 0;
        currentQuantity = startAmount;

        SetText();
    }

    /// <summary>
    /// Sets the text object to the current quantity.
    /// </summary>
    private void SetText() {
        quantityText.text = currentQuantity.ToString();
    }

    private void CheckToHide()
    {
        incrementButton.SetActive(maxQuantity > 0 && currentQuantity < maxQuantity);
        decrementButton.SetActive(currentQuantity > 1);
    }
}
