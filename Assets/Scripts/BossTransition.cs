using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTransition : MonoBehaviour
{
    // Variables
    public string bossSceneName;
    public GameObject popUp;

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
        if (other.CompareTag("Player"))
        {
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

    /// <summary>
    /// Loads the boss scene and unloads the current scene while transferring over the player's health bar and inventory system.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadBossScene()
    {
        SceneManager.LoadScene(bossSceneName, LoadSceneMode.Additive);

        // Items that should be moved over are added
        List<GameObject> itemsToCopyOver = new()
        {
            GameObject.Find("PlayerHealthBar"),
            GameObject.Find("InventorySystem"),
            GameObject.Find("EventSystem"),
            GameObject.Find("Player")
        };

        // Move the items to the new scene
        foreach (GameObject gameObject in itemsToCopyOver)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(bossSceneName));
        }

        // Reset player position and change sceneChanged bool to true
        foreach (GameObject go in SceneManager.GetSceneByName(bossSceneName).GetRootGameObjects())
        {
            if (go.name == "Player")
            {
                go.transform.position = new Vector3(0, 0, 0);
                go.GetComponent<PlayerMovementAndOxygen>().sceneChanged = true;
            }
        }

        // Set the new scene as the default and unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(bossSceneName).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(bossSceneName));

        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}
