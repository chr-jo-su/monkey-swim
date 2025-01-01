using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToBossScript : MonoBehaviour
{
    private string newSceneName = "ToCopySceneDemo";

    /// <summary>
    /// Loads in the boss scene.
    /// </summary>
    public void LoadBossScene()
    {
        StartCoroutine(LoadNewScene());
    }

    /// <summary>
    /// Loads the boss scene and unloads the current scene while transferring over the player's health bar and inventory system.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadNewScene()
    {
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Additive);

        // Add items here that should be transferred over
        List<GameObject> itemsToCopyOver = new()
        {
            GameObject.Find("PlayerHealthBar"),
            GameObject.Find("InventorySystem")
        };

        foreach (GameObject gameObject in itemsToCopyOver)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(newSceneName));
        }

        // Unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(newSceneName).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName));

        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}
