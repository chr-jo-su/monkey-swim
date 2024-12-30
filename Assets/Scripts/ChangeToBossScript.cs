using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToBossScript : MonoBehaviour {
    /// <summary>
    /// Loads in the boss scene.
    /// </summary>
    public void LoadBossScene() {
        StartCoroutine(LoadNewScene());
    }

    /// <summary>
    /// Loads the boss scene and unloads the current scene while transferring over the player's health bar and inventory system.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadNewScene() {
        SceneManager.LoadScene("ToCopySceneDemo", LoadSceneMode.Additive);

        // Add items here that should be transferred over
        List<GameObject> itemsToCopyOver = new() {
            GameObject.Find("PlayerHealthBar"),
            GameObject.Find("InventorySystem")
        };

        foreach (GameObject gameObject in itemsToCopyOver) {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("ToCopySceneDemo"));
        }

        // Unload the old scene
        while (!SceneManager.GetSceneByName("ToCopySceneDemo").isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("ToCopySceneDemo"));

        Scene oldScene = SceneManager.GetSceneByName("FromCopySceneDemo");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}
