using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSlide : MonoBehaviour
{
    public static BossSlide instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    private Rigidbody2D rb;

    private Vector3 StartPosition;
    public float DistanceOut = 8;
    private float SlideSpeed = 15;
    public bool SlideIn = false;

    public TentacleManager tent;
    public QuidManager quid;

    private bool gameOver = false;

    public AudioSource soundeffects;

    public AudioClip[] painsounds;

    public AudioClip slideInSound;
    // public AudioClip slideOutSound;

    // private bool playsound = true;
    private Camera mainCam;

    private Vector3 EndPosition;

    private Vector3 camOriginalPos;
    private float colorTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartPosition = gameObject.transform.position;
        tent.enabled = true;
        quid.enabled = false;
        SlideIn = true;
        soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
        soundeffects.PlayOneShot(slideInSound);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCam.orthographicSize = 8.0f;
        camOriginalPos = mainCam.transform.position;
        EndPosition = new Vector3(DistanceOut + StartPosition.x, StartPosition.y, StartPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        BossSlideOut();
        BossSlideIn();

        if (bossHealth.GetHealth() <= 0 && !gameOver)
        {
            TentacleManager.instance.TurnOff();
            QuidManager.instance.TurnOff();

            StartCoroutine(LoadNextLevel());
            gameOver = true;
        }

        if (Mathf.Abs(gameObject.transform.position.x - StartPosition.x) > 0.01f && Mathf.Abs(gameObject.transform.position.x - EndPosition.x) > 0.01f)
        {
            if (!mainCam)
            {
                mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                camOriginalPos = mainCam.transform.position;
            }

            mainCam.transform.localPosition = new Vector3(0, 0, -10) + (Vector3)UnityEngine.Random.insideUnitCircle * 0.2f;
        }
        else
        {
            mainCam.transform.position = camOriginalPos;
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

    public void BossSlideIn()
    {
        if (SlideIn == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                StartPosition,
                SlideSpeed * Time.unscaledDeltaTime
            );

            // if (playsound) {
            //     soundeffects.PlayOneShot(slideInSound);
            //     playsound = false;
            // }
        }

    }

    public void BossSlideOut()
    {
        if (SlideIn == true)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                EndPosition,
                SlideSpeed * Time.unscaledDeltaTime
            );

            // if (playsound) {
            //     soundeffects.PlayOneShot(slideOutSound);
            //     playsound = false;
            // }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            health -= 25;
            bossHealth.TakeDamage(BananarangDamage.instance.GetDamage());
            if (!soundeffects)
            {
                soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
            }

            int idx = UnityEngine.Random.Range(0, painsounds.Length);
            soundeffects.PlayOneShot(painsounds[idx]);

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

        TransitionManager.instance.LoadTransition("Level2");
    }
}
