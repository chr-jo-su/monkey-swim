using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossTransition : MonoBehaviour {
    // Variables
    public string bossSceneName;
    public GameObject popUp;
    public Button yesButton;
    private GameObject playerObject;

    /// <summary>
    /// Hides the pop up on start.
    /// </summary>
    public void Start()
    {
        popUp.SetActive(false);
    }

    /// <summary>
    /// Shows the pop up when the player enters the trigger box.
    /// </summary>
    /// <param name="other">The other collider that entered the trigger box.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            popUp.SetActive(true);
        }
    }


    /// <summary>
    /// Loads in the boss scene and teleports the player to the boss scene.
    /// </summary>
    public void teleportPlayerToBossScene()
    {
        StartCoroutine(LoadBossScene());
    }

    // public void teleportPlayer()
    // {
    //     //StartCoroutine(ShowTransition());
    //     playerObject.transform.position = new Vector2(120, 5);
    //     popUp.enabled = false;

    //     cam1.enabled = false;
    //     cam2.enabled = true;
    //     bossHealthThing.enabled = true;
    // }

    /// <summary>
    /// Loads the boss scene and unloads the current scene while transferring over the player's health bar and inventory system.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadBossScene() {
        SceneManager.LoadScene(bossSceneName, LoadSceneMode.Additive);

        // Add items here that should be transferred over
        List<GameObject> itemsToCopyOver = new() {
            GameObject.Find("PlayerHealthBar"),
            GameObject.Find("InventorySystem")
        };

        foreach (GameObject gameObject in itemsToCopyOver) {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(bossSceneName));
        }

        // Unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(bossSceneName).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(bossSceneName));

        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}
