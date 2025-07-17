using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSlide : MonoBehaviour
{
    // Variables
    public static BossSlide instance;

    [SerializeField] private BossHealthBar bossHealth;

    private float colorTimer = 0.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float distanceOut = 13;
    private float slideSpeed = 15;
    [HideInInspector] public bool slideIn = false;

    private bool gameOver = false;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] painSFX;
    [SerializeField] private AudioClip slideInSFX;

    private Camera mainCamera;
    private Vector3 camOriginalPos;
    public bool isLvl2;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        startPosition = gameObject.transform.position;
        TentacleManager.instance.enabled = true;
        SquidManager.instance.enabled = false;
        slideIn = true;
        sfxSource = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
        sfxSource.PlayOneShot(slideInSFX);
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 8.0f;
        camOriginalPos = mainCamera.transform.position;
        endPosition = new Vector3(distanceOut + startPosition.x, startPosition.y, startPosition.z);
    }

    // Update is called once per frame
    private void Update()
    {
        BossSlideOut();
        BossSlideIn();

        if (bossHealth.GetHealth() <= 0 && !gameOver)
        {
            TentacleManager.instance.TurnOff();
            SquidManager.instance.TurnOff();

            StartCoroutine(LoadNextLevel());
            gameOver = true;
        }

        if (Mathf.Abs(gameObject.transform.position.x - startPosition.x) > 0.01f && Mathf.Abs(gameObject.transform.position.x - endPosition.x) > 0.01f)
        {
            if (!mainCamera)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                camOriginalPos = mainCamera.transform.position;
            }

            mainCamera.transform.localPosition = new Vector3(0, 0, -10) + (Vector3)Random.insideUnitCircle * 0.2f;
        }
        else
        {
            mainCamera.transform.position = camOriginalPos;
        }

        if (colorTimer <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            colorTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Moves the boss into the screen.
    /// </summary>
    private void BossSlideIn()
    {
        if (slideIn == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                startPosition,
                slideSpeed * Time.unscaledDeltaTime
            );
        }

    }

    /// <summary>
    /// Moves the boss out of the screen.
    /// </summary>
    private void BossSlideOut()
    {
        if (slideIn == true)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                endPosition,
                slideSpeed * Time.unscaledDeltaTime
            );
        }
    }

    /// <summary>
    /// Checks whether the Bananarang hits the boss and reduces its health and plays a sound effect.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            bossHealth.TakeDamage(BananarangDamage.instance.GetDamage());
            if (!sfxSource)
            {
                sfxSource = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
            }

            int idx = Random.Range(0, painSFX.Length);
            sfxSource.PlayOneShot(painSFX[idx]);

            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
        }
    }

    /// <summary>
    /// Loads the next level and unloads the Kraken boss level.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadNextLevel()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        if (isLvl2)
            PlayerScore.instance.beatBosses[3] = true;
        if (PlayerScore.instance.toWin())
        {
            TransitionManager.instance.LoadTransition("WinGame");
        }
        TransitionManager.instance.LoadTransition("Level2");
    }
}
