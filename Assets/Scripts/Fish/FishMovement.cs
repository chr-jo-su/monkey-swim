using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 1.5f;
    public Rigidbody2D rb;
    private Transform player;

    [SerializeField]
    public int damage;
    // public int damageTaken;
    public GameObject seaLineObject;
    private GameObject healthSystem;
    private EnemyHealthBar fishHealth;
    public float detectionRadius = 4;
    public float timeToDeAggro = 1; // in seconds

    public float timeToDeAggroBrangMultiplier = 4;

    private Vector2 spawnPos;
    private bool moveLeft = false; // false == right, true == left
    private bool chasing = false;
    private float attackTimer = 0;
    private float chaseTimer = 0;
    private double sineRads = 0;
    private float colorTimer = 0;
    private float originalMoveSpeed;

    public AudioSource soundeffects;

    public AudioClip[] hurtsounds;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthSystem = transform.Find("Canvas/EnemyHealthManager").gameObject;
        fishHealth = gameObject.GetComponentInChildren<EnemyHealthBar>();
        seaLineObject = GameObject.Find("seaLine");
        originalMoveSpeed = moveSpeed;

        spawnPos = transform.position;
    }

    void Update()
    {
        CheckNull();

        // OPTIMZATION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Vector3 viewportPoint = new(0, 0, 0);
        if (Camera.current != null)
        {
            viewportPoint = Camera.current.WorldToViewportPoint(transform.position);
        }

        bool isInView = viewportPoint.x >= -0.4 && viewportPoint.x <= 1.4 &&
                        viewportPoint.y >= -0.4 && viewportPoint.y <= 1.4;
        // if (!isInView)
        // return;

        // HEALTH ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        if (fishHealth.GetHealth() <= 0.0f)
        {
            GetComponent<ItemDropper>().enabled = true;
            if (GetComponent<ItemDropper>().finished)
                Destroy(gameObject);
        }

        if (fishHealth.GetHealth() < fishHealth.GetMaxHealth())
            healthSystem.transform.localScale = new Vector3(0.2f, 0.1f, 1);

        // turn red on hit
        if (colorTimer <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            moveSpeed = originalMoveSpeed;
        }
        else
            colorTimer -= Time.deltaTime;

        // MOVEMENT ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= detectionRadius)
        {
            chasing = true;
            chaseTimer = timeToDeAggro;
        }

        if (chaseTimer <= 0 && chasing)
            chasing = false;
        else
            chaseTimer -= Time.deltaTime;

        if (chasing)
        {
            // rotate to player!
            GetComponent<SpriteRenderer>().flipX = false;
            Vector2 direction = player.position - transform.position;
            float angleRads = (float)Math.Atan2(direction.x, direction.y);
            float angleDeg = angleRads * Mathf.Rad2Deg;
            angleDeg = -angleDeg - 90;
            if (-angleDeg < 90 || -angleDeg > 270)
                GetComponent<SpriteRenderer>().flipY = false;
            else
                GetComponent<SpriteRenderer>().flipY = true;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleDeg);
            transform.rotation = targetRotation;

            // chase player!
            if (attackTimer <= 0)
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            else
                attackTimer -= Time.deltaTime;

            spawnPos.y = transform.position.y; // updating spawnPos.y so fish dont teleport back to spawn position after losing aggro
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = targetRotation;
            GetComponent<SpriteRenderer>().flipY = false;

            if (moveLeft == false)
            {
                transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.position += (Vector3)Vector2.left * moveSpeed * Time.deltaTime;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position = new Vector2(transform.position.x, spawnPos.y + 0.2f * (float)Math.Sin(sineRads));
            if (sineRads >= 2 * Math.PI)
                sineRads = 0;
            sineRads += 2 * Math.PI * Time.deltaTime; // one full sine wave per second

            if (transform.position.x < spawnPos.x - 8)
                moveLeft = false;

            if (transform.position.x > spawnPos.x + 8)
                moveLeft = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            fishHealth.TakeDamage(20);

            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
            chasing = true;
            chaseTimer = timeToDeAggro * timeToDeAggroBrangMultiplier; // chase player after hit
            moveSpeed = 0;

            // Vector2 dir = -(collision.transform.position - transform.position);
            if (!soundeffects)
                soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
                    
            int idx = UnityEngine.Random.Range(0, hurtsounds.Length);
            soundeffects.PlayOneShot(hurtsounds[idx], 0.5f);
        }

        // yikes ;; switch direction after colliding with primitive shapes (tim)
        if (collision.gameObject.name.Contains("Square")
                || collision.gameObject.name.Contains("Triangle")
                || collision.gameObject.name.Contains("Hexagon")
                || collision.gameObject.name.Contains("seaLine")
                || collision.gameObject.name.Contains("Terrain"))
        {
            moveLeft = !moveLeft;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            if (attackTimer <= 0)
            {
                PlayerHealthBar.instance.TakeDamage(damage);
                attackTimer += 1;
            }
        }
    }

    void CheckNull()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}