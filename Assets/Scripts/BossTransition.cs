using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossTransition : MonoBehaviour {
    public Canvas popUp;
    public Button yesButton;
    private GameObject playerObject;

    private AsyncOperation asyncLoadLevel;

    public void Start() {
        popUp.enabled = false;
        yesButton.onClick.AddListener(teleportPlayer);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // other.transform.position = new Vector2(100, 100);
            playerObject = other.gameObject;
            popUp.enabled = true;
        }
    }


    public void teleportPlayer() {
        bool mainSceneChanged = false;

        StartCoroutine(ShowTransition());

        int delay = 0;
        while (delay < 2000) {
            delay += (int)(Time.unscaledDeltaTime * 1000);

            if (!mainSceneChanged && delay > 1500) {
                playerObject.transform.position = new Vector2(100, 100);
                mainSceneChanged = true;
            }
        }

        popUp.enabled = false;
    }

    IEnumerator ShowTransition() {
        asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) yield return null;

        SceneManager.GetSceneByName("TransitionScene").GetRootGameObjects()[1].GetComponent<TransitionManager>().LoadTransition(2000);
    }
}
