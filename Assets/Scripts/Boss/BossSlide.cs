using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class BossSlide : MonoBehaviour
{
    public static BossSlide instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    private Rigidbody2D rb;

    private Vector3 StartPosition;
    private float DistanceOut = 22;
    private float SlideSpeed = 15;
    public bool SlideIn = false;

    public TentacleManager tent;
    public QuidManager quid;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        tent.enabled = true;
        quid.enabled = false;
        SlideIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        BossSlideOut();
        BossSlideIn();
        // if (bossHealth.GetHealth() < bossHealth.GetMaxHealth()/2)
        // {
        //     tent.enabled = false;
        //     quid.enabled = true;
        // }

        if (bossHealth.GetHealth() <= 0 && !gameOver)
        {
            TentacleManager.instance.TurnOff();
            QuidManager.instance.TurnOff();

            StartCoroutine(LoadWinScreen());
            gameOver = true;
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
        }
    }

    public void BossSlideOut()
    {
        if (SlideIn == true)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                new Vector3(DistanceOut, StartPosition.y, StartPosition.z),
                SlideSpeed * Time.unscaledDeltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            health -= 25;
            bossHealth.TakeDamage(BanarangDamage.instance.GetDamage());
        }
    }

    /// <summary>
    /// Loads the win scene and unloads the current scene.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadWinScreen()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        TransitionManager.instance.LoadTransition("WinLvl1");
    }
}
