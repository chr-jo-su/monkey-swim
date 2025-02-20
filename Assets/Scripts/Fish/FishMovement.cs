using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FishMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 1.5f;
    public Rigidbody2D rb;
    private Transform player;

    [SerializeField]
    public int damage;
    public int damageTaken;
    public GameObject seaLineObject;
    private GameObject healthSystem;
    public float health = 100.0f;
    private float healthMax;
    private EnemyHealthBar fishHealth;

    private Vector2 spawnPos;
    private bool moveLeft = false; // false == right, true == left
    private bool chasing = false;
    private float attackTimer = 0;

    void Start()
    {
        healthMax = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthSystem = transform.Find("Canvas/EnemyHealthManager").gameObject;
        fishHealth = gameObject.GetComponentInChildren<EnemyHealthBar>();
        seaLineObject = GameObject.Find("seaLine");

        spawnPos = transform.position;
    }

    void Update()
    {

        // HEALTH ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        if (health <= 0.0f)
        {
            GetComponent<ItemDropper>().enabled = true;
            if (GetComponent<ItemDropper>().finished)
                Destroy(gameObject);
        }

        if (health < healthMax)
        {
            healthSystem.transform.localScale = new Vector3(0.2f, 0.1f, 1);
        }

        // MOVEMENT ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 7) {
            chasing = true;
        }

        if (chasing) {
            // rotate to player!
            GetComponent<SpriteRenderer>().flipX = false;
            Vector2 direction = player.position - transform.position;
            float angleRads = (float)Math.Atan2(direction.x, direction.y);
            float angleDeg = angleRads * Mathf.Rad2Deg;
            angleDeg = -angleDeg - 90;
            if (-angleDeg < 90 || -angleDeg > 270) {
                GetComponent<SpriteRenderer>().flipY = false;
            } else {
                GetComponent<SpriteRenderer>().flipY = true;
            }
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleDeg);
            transform.rotation = targetRotation;

            // chase player!
            if (attackTimer <= 0) {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed*Time.deltaTime);
            } else {
                attackTimer -= Time.deltaTime;
            }
        } else {
            if (moveLeft == false)
                transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
            else
                transform.position += (Vector3)Vector2.left * moveSpeed * Time.deltaTime;

            if (transform.position.x < spawnPos.x - 8) {
                moveLeft = false;
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (transform.position.x > spawnPos.x + 8) {
                moveLeft = true;
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            health -= damageTaken;
            fishHealth.TakeDamage(damageTaken);
        }

        // yikes ;; switch direction after colliding with primitive shapes (tim)
        if (collision.gameObject.name.Contains("Square")
                || collision.gameObject.name.Contains("Triangle")
                || collision.gameObject.name.Contains("Hexagon")
                || collision.gameObject.name.Contains("seaLine"))
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
            if (attackTimer <= 0) {
                PlayerHealthBar.instance.TakeDamage(damage);
                attackTimer += 1;
            }
        }
    }

}