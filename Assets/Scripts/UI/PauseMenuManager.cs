using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    // Variables
    public GameObject pauseMenu;
    public KeyCode pauseMenuKey = KeyCode.Escape;
    [SerializeField] private float velocity = 5f;
    [HideInInspector] public bool paused;

    private Vector2 showingPos = new(0, 0);
    private Vector2 hiddenPos;
    private Vector2 targetPos;

    /// <summary>
    /// Unpauses the game on start.
    /// </summary>
    void Start()
    {
        paused = true;
        hiddenPos = new(0, Screen.height * 1.5f);
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
            targetPos = hiddenPos;
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
            targetPos = showingPos;
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
    /// Loads the title screen asynchronously.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevel()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) yield return null;

        TransitionManager.instance.LoadTransition("TitleScreen");
    }

    /// <summary>
    /// Animates the opening and closing of the pause menu.
    /// </summary>
    private void AnimateMenu()
    {
        // Animate the menu moving
        pauseMenu.transform.localPosition = Vector3.Lerp(pauseMenu.transform.localPosition, targetPos, velocity * Time.unscaledDeltaTime);
    }
}