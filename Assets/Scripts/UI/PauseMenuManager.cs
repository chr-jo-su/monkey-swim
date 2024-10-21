using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuMovement : MonoBehaviour
{
    // Variables
    public GameObject pauseMenu;
    public KeyCode pauseMenuKey = KeyCode.Escape;
    public float velocity = 5f;
    private bool paused;
    private Vector3 openPos = new(Screen.width / 2, Screen.height / 2, 0);
    private Vector3 closePos = new(Screen.width / 2, Screen.height * 2, 0);
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        paused = true;
        UnpauseGame();
    }

    // Update is called once per frame
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
    /// Opens the main menu scene.
    /// </summary>
    public void GoToMain()
    {
        SceneManager.LoadScene("SampleScene");
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
