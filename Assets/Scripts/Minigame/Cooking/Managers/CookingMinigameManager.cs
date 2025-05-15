using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CookingMinigameManager : MonoBehaviour
{
    // Variables
    private float timer = 0f;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool isGameOver = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winScreenText;

    [SerializeField] private TextMeshProUGUI currentRecipeTitleText;

    [SerializeField] private List<CookingRecipe> recipeList;
    private int currentRecipeIndex = -1;

    // Create a singleton instance of this game
    public static CookingMinigameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        winScreen.SetActive(false);
        ShowNextRecipe();
    }

    private void FixedUpdate()
    {
        UpdateTimer();
        CheckGameOver();
    }

    public void CheckIsRecipeCompleted()
    {
        CookingAreaManager.instance.UnhighlightAllSlots();

        bool completed = true;

        //TODO: Do checks that check if the user put the correct things into the correct area
        List<CookingIngredient> currentItems = CookingAreaManager.instance.GetCurrentItems();

        for (int i = 0; i < currentItems.Count; i++)
        {
            if (currentItems[i] != recipeList[currentRecipeIndex].ingredients[i])
            {
                CookingAreaManager.instance.HighlightSlot(i);
                completed = false;
            }
        }

        if (completed)
        {
            ShowNextRecipe();
        }
    }

    private void ShowNextRecipe()
    {
        currentRecipeIndex++;

        if (currentRecipeIndex >= recipeList.Count)
        {
            ShowEndScreen(true);
        }
        else
        {
            timer = recipeList[currentRecipeIndex].maxTime;

            ReferenceRecipeManager.instance.UpdateSlots(recipeList[currentRecipeIndex].ingredients);

            currentRecipeTitleText.text = "Time to make " + recipeList[currentRecipeIndex].recipeName + "!";

            CookingAreaManager.instance.ClearSlots();
            PrepAreaSlotHolder.instance.RemoveIngredients();
        }
    }

    private void UpdateTimer()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
        }
    }

    private void CheckGameOver()
    {
        if (timer <= 0)
        {
            isGameOver = true;
            ShowEndScreen(false);
        }
    }

    private void ShowEndScreen(bool isWin)
    {
        winScreen.SetActive(true);

        if (isWin)
        {
            winScreenText.text = "You win!";
        }
        else
        {
            winScreenText.text = "You lose!";
        }
    }
}
