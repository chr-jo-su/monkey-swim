using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    // Variables
    [SerializeField] private Image creditsBackground;
    [SerializeField] private Image titleBackground;
    [SerializeField] private GameObject credits;

    private Color startColour = new(1, 1, 1, 0);
    private Color endColour = new(1, 1, 1, 1);

    private bool fadedIn = false;
    [SerializeField] private float fadeInTime;
    private float fadeVelocity;

    [SerializeField] private Vector3 endPosition;
    private bool scrollFinished = false;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float acceleration;
    private float currentScrollSpeed = 0f;

    private bool transitioned = false;
    [SerializeField] private float transitionHoldTime = 1.5f;

    private bool titleShown = false;
    [SerializeField] private float titleHoldTime = 1.5f;

    private bool complete = false;
    [SerializeField] private float endHoldTime = 2f;

    private float holdTimer = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        creditsBackground.color = startColour;
        titleBackground.color = startColour;
        fadeVelocity = 1 / fadeInTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (holdTimer < transitionHoldTime && !transitioned)
        {
            holdTimer += Time.deltaTime;
            return;
        }

        transitioned = true;

        if (!fadedIn)
        {
            titleBackground.color += new Color(1, 1, 1, fadeVelocity);

            if (titleBackground.color.a + 0.01f >= endColour.a)
            {
                creditsBackground.color = endColour;
                titleBackground.color = endColour;
                holdTimer = 0;
                fadedIn = true;
            }

            return;
        }

        if (holdTimer < titleHoldTime && !titleShown)
        {
            holdTimer += Time.deltaTime;
            return;
        }

        titleShown = true;

        if (!scrollFinished)
        {
            credits.transform.Translate(currentScrollSpeed * Time.deltaTime * Vector3.up, Space.World);
            currentScrollSpeed = Mathf.Clamp(currentScrollSpeed + (acceleration * Time.deltaTime), 0, scrollSpeed);

            if (credits.transform.position.y - endPosition.y >= Camera.main.scaledPixelHeight)
            {
                holdTimer = 0;
                scrollFinished = true;
            }

            return;
        }

        if (holdTimer < endHoldTime && !complete)
        {
            holdTimer += Time.deltaTime;
            return;
        }

        if (scrollFinished && !complete)
        {
            complete = true;
            StartCoroutine(LoadTitleScreen());
        }
    }

    /// <summary>
    /// Loads the title screen asynchronously.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadTitleScreen()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) yield return null;

        TransitionManager.instance.LoadTransition("TitleScreen");
    }
}
