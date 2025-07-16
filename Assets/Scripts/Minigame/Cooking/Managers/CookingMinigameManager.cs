using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CookingMinigameManager : MonoBehaviour
{
    // Variables
    private float timer = 0f;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool hasGameStarted = false;
    private bool isGameOver = false;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winScreenText;

    [SerializeField] private TextMeshProUGUI currentRecipeTitleText;

    [SerializeField] private List<CookingRecipe> recipeList;
    private int currentRecipeIndex = -1;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> dropSFX;
    [SerializeField] private List<AudioClip> recipeSubmitCorrectSFX;
    [SerializeField] private List<AudioClip> recipeSubmitIncorrectSFX;

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
            PlayRecipeSubmitSFX(true);
        }
        else
        {
            PlayRecipeSubmitSFX(false);
        }
    }

    private void ShowNextRecipe()
    {
        currentRecipeIndex++;

        if (currentRecipeIndex >= recipeList.Count)
        {
            isGameOver = true;
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
        if (!isGameOver && hasGameStarted)
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

    public void PlayClickSFX()
    {
        sfxSource.PlayOneShot(dropSFX.Count > 0 ? dropSFX[Random.Range(0, dropSFX.Count)] : null);
    }

    public void PlayRecipeSubmitSFX(bool isCorrect)
    {
        if (isCorrect)
        {
            sfxSource.PlayOneShot(recipeSubmitCorrectSFX.Count > 0 ? recipeSubmitCorrectSFX[Random.Range(0, recipeSubmitCorrectSFX.Count)] : null);
        }
        else
        {
            sfxSource.PlayOneShot(recipeSubmitIncorrectSFX.Count > 0 ? recipeSubmitIncorrectSFX[Random.Range(0, recipeSubmitIncorrectSFX.Count)] : null);
        }
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }

    public void RestartMinigame()
    {
        currentRecipeIndex = -1;
        ShowNextRecipe();

        winScreen.SetActive(false);
        startScreen.SetActive(true);

        hasGameStarted = false;
        isGameOver = false;
    }

    public void ExitMinigame()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void StartGame()
    {
        PlayClickSFX();

        startScreen.SetActive(false);
        hasGameStarted = true;
    }

    /// <summary>
    /// Loads the main menu scene and unloads the current scene.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        TransitionManager.instance.LoadTransition("TitleScreen");
    }
}
