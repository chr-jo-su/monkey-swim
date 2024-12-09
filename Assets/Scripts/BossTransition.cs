using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTransition : MonoBehaviour
{
    public Canvas popUp;
    public Button yesButton;
    private GameObject playerObject;

    public void Start()
    {
        popUp.enabled = false;
        yesButton.onClick.AddListener(teleportPlayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // other.transform.position = new Vector2(100, 100);
            playerObject = other.gameObject;
            popUp.enabled = true;
        }
    }


    public void teleportPlayer()
    {
        //StartCoroutine(ShowTransition());
        playerObject.transform.position = new Vector2(100, 100);
        popUp.enabled = false;
    }

    //private IEnumerator ShowTransition()
    //{
    //    asycnLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

    //    while (!asycnLoadLevel.isDone) yield return null;

    //    SceneManager.GetSceneByName("TransitionScene").GetRootGameObject()[1].GetComponent<TransitionManager>().LoadTransition(2000);
    //}
}
