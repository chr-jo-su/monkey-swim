using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeaGoatManager : MonoBehaviour
{
    // Variables
    public static SeaGoatManager instance;

    [SerializeField] private BossHealthBar bossHealth;
    [SerializeField] private float stage2Health;
    private bool stage2Started = false;

    [SerializeField] private AudioSource soundeffects;
    [SerializeField] private AudioClip[] painsounds;
    private float colorTimer = 0.0f;

    private bool canBeHurt = false;
    private bool canDamage = true;

    [HideInInspector] public StageType stage;

    [HideInInspector] public float regularBossScale = 0.35f;
    [HideInInspector] public float dashBossScale = 0.50f;
    [HideInInspector] public float hornMissileBossScale = 0.25f;

    public GameObject leftHornMissilePrefab;
    public GameObject rightHornMissilePrefab;
    private bool gameOver = false;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        OxygenBar.instance.SetBreathe(false);       // As we start in the sea, it doesn't auto update (this might be removed later as the player is teleported from other levels to here)
    }

    public void Update()
    {
        if (bossHealth.GetHealth() <= 0 && !gameOver)
        {
            StartCoroutine(LoadNextLevel());
            gameOver = true;
        }

        if (bossHealth.GetHealth() <= stage2Health && !stage2Started)
        {
            //TODO: Instantiate the two separate bosses and destroy the current game object (and any horn missiles that are in the scene)
            stage2Started = true;
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

    public void ChangeStage(StageType type)
    {
        stage = type;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)" && canBeHurt)
        {
            bossHealth.TakeDamage(BananarangDamage.instance.GetDamage());
            if (!soundeffects)
                soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();

            int idx = Random.Range(0, painsounds.Length);
            soundeffects.PlayOneShot(painsounds[idx]);

            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
        }
        else if (collision.gameObject.name == "Player" && canDamage)
        {
            PlayerHealthBar.instance.TakeDamage(25);
        }
    }

    public void SetCanBeHurt(bool canBeHurt)
    {
        this.canBeHurt = canBeHurt;
    }

    public void SetCanDamage(bool canDamage)
    {
        this.canDamage = canDamage;
    }

    private IEnumerator LoadNextLevel()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        PlayerScore.instance.beatBosses[2] = true;
        if (PlayerScore.instance.toWin())
        {
            TransitionManager.instance.LoadTransition("WinGame");
        }
        TransitionManager.instance.LoadTransition("Level2");
        // return null;
    }
}

public enum StageType
{
    None,
    Start,
    Idle,
    Dash,
    HornMissile
}
