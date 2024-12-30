using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {
    // Variables
    public static TransitionManager instance;
    public GameObject transitionScreen;
    public float velocity = 10f;

    private bool allowLoad = false;
    private bool allowAnimation = false;
    private bool completedLoading = false;
    private bool canClose = false;

    private Vector2 hiddenPos;
    private Vector2 showingPos;
    private Vector2 targetPos;

    private string sceneName;
    private bool sameScene;
    private int delay;
    private GameObject gameObjectToDestroy;
    private List<GameObject> toCopyOver;

    /// <summary>
    /// Hides the transition screen on start and sets the positions for the start and end.
    /// </summary>
    private void Awake() {
        instance = this;
        hiddenPos = new(transitionScreen.transform.position.x, transitionScreen.transform.position.y);
        showingPos = new(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        HideTransitionScreen();
    }

    /// <summary>
    /// Handles transition animations and boolean checking for certin stages.
    /// </summary>
    private void Update() {
        AnimateMenu();

        if (allowAnimation && !completedLoading) {
            if (Mathf.Round(transitionScreen.transform.position.y) == Camera.main.pixelHeight / 2) {
                allowLoad = true;
            } else {
                allowLoad = false;
            }

            if (!allowLoad) {
                ShowTransitionScreen();
            } else {
                if (!sameScene) {
                    Destroy(gameObjectToDestroy);

                    StartCoroutine(LoadNewScene());
                } else {
                    if (delay > 0) {
                        delay -= (int)(Time.unscaledDeltaTime * 1000);
                    } else {
                        completedLoading = true;
                    }
                }
            }
        } else if (completedLoading && canClose) {
            // Unload the transition screen
            if (sameScene) {
                UnloadTransitionScene();
            } else {
                UnloadOtherScenes();
            }
        } else {
            HideTransitionScreen();
        }

        if (!canClose && completedLoading && Mathf.Round(transitionScreen.transform.position.y) == hiddenPos.y) {
            canClose = true;
        }
    }

    /// <summary>
    /// Animates the transition screen to targetPos.
    /// </summary>
    private void AnimateMenu() {
        transitionScreen.transform.position = Vector3.Lerp(transitionScreen.transform.position, targetPos, velocity * Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Changes the target positioning to fill the camera frame.
    /// </summary>
    private void ShowTransitionScreen() {
        if (!completedLoading) {
            targetPos = showingPos;
        }
    }

    /// <summary>
    /// Changes the target positioning to out of the camera frame.
    /// </summary>
    private void HideTransitionScreen() {
        targetPos = hiddenPos;
    }

    /// <summary>
    /// Starts the transition to switch to sceneName with given name.
    /// </summary>
    /// <param name="sceneName">The name of the sceneName to be loaded as a string.</param>
    public void LoadTransition(string sceneName, GameObject gameObjectToDestroy = null, List<GameObject> toCopyOver = null) {
        allowAnimation = true;
        sameScene = false;

        // Show the transition screen
        ShowTransitionScreen();

        // Save info to use later
        this.sceneName = sceneName;
        this.gameObjectToDestroy = gameObjectToDestroy;
        this.toCopyOver = toCopyOver;
    }

    /// <summary>
    /// Starts the transition and waits the given delay.
    /// </summary>
    /// <param name="delay">The delay to wait (given in milliseconds) between opening and closing the transition scene.</param>
    public void LoadTransition(int delay) {
        allowAnimation = true;
        sameScene = true;
        this.delay = delay;

        ShowTransitionScreen();
    }

    /// <summary>
    /// Loads the stored sceneName and finishes the transition.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNewScene() {
        List<AsyncOperation> asyncOperations = UnloadOtherScenes();

        while (!asyncOperations.TrueForAll(operation => operation.isDone)) yield return null;

        // Load the next sceneName
        SceneManager.LoadScene(this.sceneName, LoadSceneMode.Additive);

        // Add items here, if any
        foreach (GameObject gameObject in toCopyOver) {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(this.sceneName));
        }

        completedLoading = true;
    }

    /// <summary>
    /// Unloads all scenes except the last one.
    /// </summary>
    /// <returns>A list of async operations for unloading scenes that are being run.</returns>
    private List<AsyncOperation> UnloadOtherScenes() {
        List<AsyncOperation> asyncOperations = new();

        // Unload previous scenes
        for (int i = SceneManager.sceneCount - 2; i >= 0; i--) {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
        }

        return asyncOperations;
    }

    /// <summary>
    /// Unloads the transition scene.
    /// </summary>
    /// <returns>The transition scene unload operation if found, otherwise null.</returns>
    private AsyncOperation UnloadTransitionScene() {
        AsyncOperation asyncOperations = null;

        // Unload transition scene
        for (int i = SceneManager.sceneCount; i >= 0; i--) {
            if (SceneManager.GetSceneAt(i).name == "TransitionScene") {
                asyncOperations = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }

        return asyncOperations;
    }
}
