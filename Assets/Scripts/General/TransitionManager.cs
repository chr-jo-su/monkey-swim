using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // Variables
    public static TransitionManager instance;

    public GameObject transitionScreen;
    public float velocity = 5f;

    private bool canLoadNewScene = true;
    private bool canCopyOver = true;
    private bool canUnloadOldScenes = true;
    private bool endTransition = false;

    private Vector2 hiddenPos;
    private Vector2 showingPos = new(0, 0);
    private Vector2 targetPos;

    private string sceneName;
    private List<GameObject> toCopyOver;
    private bool runSpecialCode;

    /// <summary>
    /// Hides the transition screen on start and sets the positions for the start and end.
    /// </summary>
    private void Awake()
    {
        instance = this;
        hiddenPos = new(0, transitionScreen.transform.localPosition.y);

        HideTransitionScreen();
    }

    /// <summary>
    /// Handles transition animations and boolean checking for certin stages.
    /// </summary>
    private void Update()
    {
        // Check if the transition screen is fully up if it's starting the transition
        if (!IsTransitionScreenUp() && !endTransition)
        {
            AnimateMenu();
        }
        // Transition screen is fully up at this point
        else if (!endTransition)
        {
            // Transition screen is covering everything at this point

            // Check if the new scene can/needs to be loaded
            if (canLoadNewScene)
            {
                LoadNewScene();
                canLoadNewScene = false;

            }
            // Check if the new scene has been loaded in
            else if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                // New scene has been loaded in at this point


                // Check if items can/need to be copied over
                if (canCopyOver)
                {
                    CopyOverItems();
                    canCopyOver = false;
                }
                else if (IsCopiedOver())
                {
                    // Items have been copied over at this point

                    // Check if the other scenes can be unloaded (not including the new or transition scene)
                    if (canUnloadOldScenes)
                    {
                        StartCoroutine(UnloadOldScenes());

                        if (runSpecialCode)
                        {
                            RunSpecialCode();
                        }

                        canUnloadOldScenes = false;
                    }
                    else if (SceneManager.sceneCount == 2)
                    {
                        // All other scenes have been unloaded at this point
                        HideTransitionScreen();

                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

                        Time.timeScale = 1f;

                        // Change the active camera to the new scene's camera
                        //Camera.main.gameObject.SetActive(false);
                        Camera.allCameras[0].gameObject.SetActive(true);

                        endTransition = true;
                    }
                }
            }
        }
        else
        {
            // Everything is done transferring and loading, so move the transition screen down

            // Check if the transition screen is fully down
            if (IsTransitionScreenDown())
            {
                UnloadTransitionScene();
            }
            else
            {
                AnimateMenu();
            }
        }
    }

    /// <summary>
    /// Animates the transition screen to targetPos.
    /// </summary>
    private void AnimateMenu()
    {
        transitionScreen.transform.localPosition = Vector3.Lerp(transitionScreen.transform.localPosition, targetPos, velocity * Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Changes the target positioning to fill the camera frame.
    /// </summary>
    private void ShowTransitionScreen()
    {
        targetPos = showingPos;
    }

    /// <summary>
    /// Changes the target positioning to out of the camera frame.
    /// </summary>
    private void HideTransitionScreen()
    {
        targetPos = hiddenPos;
    }

    /// <summary>
    /// Checks if the transition screen is up.
    /// </summary>
    /// <returns>A boolean value whether the transition screen is fully showing or not.</returns>
    private bool IsTransitionScreenUp()
    {
        return Mathf.Round(transitionScreen.transform.localPosition.y) == showingPos.y;
    }

    /// <summary>
    /// Checks if the transition screen is down.
    /// </summary>
    /// <returns>A boolean value whether the transition screen is fully hidden or not.</returns>
    private bool IsTransitionScreenDown()
    {
        return Mathf.Round(transitionScreen.transform.localPosition.y) == hiddenPos.y;
    }

    /// <summary>
    /// Starts the transition to switch to sceneName with given name.
    /// </summary>
    /// <param name="sceneName">The name of the sceneName to be loaded as a string.</param>
    public void LoadTransition(string sceneName, List<GameObject> toCopyOver = null, bool runSpecialCode = false)
    {
        // Show the transition screen
        ShowTransitionScreen();

        // Save info to use later
        this.sceneName = sceneName;
        this.toCopyOver = toCopyOver;
        this.runSpecialCode = runSpecialCode;

    }

    /// <summary>
    /// Loads the new scene that matches the given sceneName.
    /// </summary>
    private void LoadNewScene()
    {
        // Load the next sceneName
        SceneManager.LoadScene(this.sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Copies over items from the old scene into the new scene if there are any.
    /// </summary>
    private void CopyOverItems()
    {
        // Add items here, if any
        if (toCopyOver != null)
        {
            foreach (GameObject gameObject in toCopyOver)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(this.sceneName));
            }
        }
    }

    /// <summary>
    /// Checks if all items that need to be copied over have been copied over.
    /// </summary>
    /// <returns>A boolean value whether the items have all been copied over or not.</returns>
    private bool IsCopiedOver()
    {
        if (toCopyOver != null)
        {
            return toCopyOver.TrueForAll(gameObject => gameObject.scene == SceneManager.GetSceneByName(this.sceneName));
        }

        return true;
    }

    /// <summary>
    /// This is run when the player is teleported to the boss level.
    /// </summary>
    private void RunSpecialCode()
    {
        // Reset player position and change sceneChanged bool to true
        foreach (GameObject go in SceneManager.GetSceneByName(sceneName).GetRootGameObjects())
        {
            if (go.name == "Player")
            {
                go.transform.position = new Vector3(0, 0, 0);
                go.GetComponent<PlayerMovement>().sceneChanged = true;
            }
        }
    }

    /// <summary>
    /// Unloads all scenes except the transition scene and the newly loaded scene.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnloadOldScenes()
    {
        List<AsyncOperation> asyncOperations = new();

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));

        // Unload previous scenes
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
        {
            if (SceneManager.GetSceneAt(i).name != "TransitionScene" && SceneManager.GetSceneAt(i).name != this.sceneName)
            {
                asyncOperations.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
            }
        }

        while (!asyncOperations.TrueForAll(operation => operation.isDone)) yield return null;
    }

    /// <summary>
    /// Unloads the transition scene.
    /// </summary>
    /// <returns>The transition scene unload operation.</returns>
    private AsyncOperation UnloadTransitionScene()
    {
        return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("TransitionScene"));
    }
}
