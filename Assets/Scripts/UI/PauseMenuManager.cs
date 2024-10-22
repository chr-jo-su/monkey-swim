using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    // Variables
    public GameObject pauseMenu;
    public KeyCode pauseMenuKey = KeyCode.Escape;
    public float velocity = 5f;
    private bool paused;

    [HideInInspector] public Vector3 openPos = new(Screen.width / 2, Screen.height / 2, 0);
    [HideInInspector] public Vector3 closePos = new(Screen.width / 2, Screen.height * 2, 0);
    private Vector3 targetPos;

    private AsyncOperation asyncLoadLevel;

    /// <summary>
    /// Unpauses the game on start.
    /// </summary>
    void Start()
    {
        paused = true;
        UnpauseGame();
    }

    /// <summary>
    /// Handles animations and keyboard inputs.
    /// </summary>
    void Update()
    {
        AnimateMenu();

        // Check for keyboard inputs
        CheckKeyboardInputs();

        if (pauseMenu.transform.position.y - Camera.main.pixelHeight >= Camera.main.pixelHeight / 2)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Checks for keyboard inputs.
    /// </summary>
    private void CheckKeyboardInputs()
    {
        // Check for the pause menu key to be pressed
        if (Input.GetKeyDown(pauseMenuKey))
        {
            if (paused)
            {
                // If the game's paused, unpause it
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Unpauses the game.
    /// </summary>
    public void UnpauseGame()
    {
        if (paused)
        {
            targetPos = closePos;
            Time.timeScale = 1f;
            paused = false;
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        if (!paused)
        {
            targetPos = openPos;
            Time.timeScale = 0f;
            paused = true;
        }
    }

    /// <summary>
    /// Opens the main menu sceneName.
    /// </summary>
    public void LoadTitleScreen()
    {
        StartCoroutine(LoadLevel());
    }

    /// <summary>
    /// Loads the level asynchronously.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevel()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) yield return null;

        SceneManager.GetSceneByName("TransitionScene").GetRootGameObjects()[1].GetComponent<TransitionManager>().LoadTransition("TitleScreen", pauseMenu);
    }

    /// <summary>
    /// Animates the opening and closing of the pause menu.
    /// </summary>
    private void AnimateMenu()
    {
        // Animate the menu moving
        pauseMenu.transform.position = Vector3.Lerp(pauseMenu.transform.position, targetPos, velocity * Time.unscaledDeltaTime);
    }
}
