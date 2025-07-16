using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject comicAdvanceButton;
    [SerializeField] private float velocity = 10;
    private bool startPressed = false;

    // Update is called once per frame
    void Update()
    {
        if (startPressed == true)
        {
            HideMainTitleMenu();
            comicAdvanceButton.SetActive(true);
        }
        else
        {
            comicAdvanceButton.SetActive(false);
        }

        if (transform.localPosition.y - Camera.main.pixelHeight >= Camera.main.pixelHeight * (2 / 3))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Hides the main title menu by moving it off-screen
    /// </summary>
    public void HideMainTitleMenu()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(0, Screen.height * 1.5f), velocity * Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Starts the game by setting the startPressed flag to true
    /// </summary>
    public void StartGame()
    {
        startPressed = true;
    }

    /// <summary>
    /// Exit the game application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens the credits scene and starts showing credits.
    /// </summary>
    public void ShowCredits()
    {
        StartCoroutine(LoadScene("Credits"));
    }

    /// <summary>
    /// Opens the minigame and starts it.
    /// </summary>
    public void StartMinigame()
    {
        StartCoroutine(LoadScene("CookingMinigame"));
    }

    /// <summary>
    /// Loads the given scene and unloads the current scene.
    /// </summary>
    /// <param name="sceneName">The scene name to load next.</param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        TransitionManager.instance.LoadTransition(sceneName);
    }
}
