using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTransition : MonoBehaviour
{
    public Canvas popUp;
    public Button yesButton;
    private GameObject playerObject;
    public Camera cam1;
    public Camera cam2;
    public Canvas bossHealthThing;
    // public TentacleManager tent;
    // public BossSlide boss;


    public void Start()
    {
        popUp.enabled = false;
        yesButton.onClick.AddListener(teleportPlayer);
        cam1.enabled = true;
        cam2.enabled = false;
        bossHealthThing.enabled = false;
        // tent.enabled = false;
        // boss.enabled = false;
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
        playerObject.transform.position = new Vector2(120, 5);
        popUp.enabled = false;

        cam1.enabled = false;
        cam2.enabled = true;
        bossHealthThing.enabled = true;
        // boss.enabled = true;
    }

    //private IEnumerator ShowTransition()
    //{
    //    asycnLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

    //    while (!asycnLoadLevel.isDone) yield return null;

    //    SceneManager.GetSceneByName("TransitionScene").GetRootGameObject()[1].GetComponent<TransitionManager>().LoadTransition(2000);
    //}
}
