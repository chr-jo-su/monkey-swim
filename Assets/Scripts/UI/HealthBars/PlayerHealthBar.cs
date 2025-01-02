using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    public static PlayerHealthBar instance;
    private bool gameOver = false;

    private void Awake()
    {
        instance = this;
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
 
        if (base.GetHealth() <= 0 && !gameOver)
        {
            StartCoroutine(LoadGameOverScreen());
            gameOver = true;
        }
    }

    /// <summary>
    /// Loads the game over scene and unloads the current scene.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadGameOverScreen()
    {
        string gameOverScene = "GameOver";
        SceneManager.LoadScene(gameOverScene, LoadSceneMode.Additive);

        // Set the new scene as the default and unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(gameOverScene).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameOverScene));

        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}