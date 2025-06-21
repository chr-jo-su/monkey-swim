using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    public static PlayerMovement instance;

    public float moveSpeed = 5f;
    public Rigidbody2D rigidBody;
    public GameObject seaLineObject; // should be a thin object with a boxCollider2D component and trigger enabled
                                     // that signifies the sea line
    Vector2 movement;
    public Animator animator;

    [HideInInspector] public bool sceneChanged = true;
    private bool inSea = false;
    private bool canMoveUp = true;

    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip itemPickupSound;
    public AudioSource seaAmbience;
    public AudioSource underWaterAmbience;
    public AudioSource underWaterMusic;
    public Collider2D seaTopBoxCollider;
    public Collider2D playerCollider;

    private bool facingLeft = true;

    private float colorTimer = 0;

    public AudioSource soundeffects;
    public AudioClip[] painsounds;

    private float damageTimerBoss = 0.0f;

    /// <summary>
    /// Creates a singleton instance of the PlayerMovement.
    /// </summary>
    private void Awake()
    {
        instance = this;

        // GameObject water = GameObject.FindGameObjectWithTag("Water");
        // Color prevColor = water.GetComponent<SpriteRenderer>().color;
        // water.GetComponent<SpriteRenderer>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 0.96f);
    }

    /// <summary>
    /// Sets the collision between the player and the sea line to be ignored.
    /// </summary>
    void Start()
    {
        Physics2D.IgnoreCollision(seaTopBoxCollider, playerCollider, true);
    }

    /// <summary>
    /// Relinks the player's attributes if they are missing and processes the inputs for player movement.
    /// </summary>
    void Update()
    {
        if (sceneChanged)
        {
            RelinkAttributes();
        }

        ProcessInputs();

        if (colorTimer <= 0)
            GetComponent<SpriteRenderer>().color = Color.white;
        else
            colorTimer -= Time.deltaTime;

        if (damageTimerBoss > 0.0f)
            damageTimerBoss -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        Move();

        if (transform.position.y > seaLineObject.transform.position.y)
        {
            inSea = false;
        }
        else
        {
            inSea = true;
        }
    }

    /// <summary>
    /// Process the inputs for the player.
    /// </summary>
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");


        if (facingLeft == false)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;


        if (moveX < 0)
            facingLeft = true;
        else if (moveX > 0)
            facingLeft = false;

        animator.SetFloat("Speed", Mathf.Abs(moveX));

        movement = new Vector2(moveX, moveY).normalized;
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    void Move()
    {
        //rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        Vector2 v = new Vector2(movement.x, movement.y);

        if (canMoveUp)
        {
            v = new Vector2(movement.x, movement.y);
        }
        else
        {
            v = new Vector2(movement.x, 0);
        }

        rigidBody.AddForce(v.normalized * moveSpeed);
    }

    /// <summary>
    /// Changes the move speed of the player by the given value.
    /// </summary>
    /// <param name="val">The value to change the move speed by. Can be negative.</param>
    public void ChangeMoveSpeed(int val)
    {
        moveSpeed += val;
    }

    /// <summary>
    /// Changes the player's ability to breathe and changes the gravity scale of the player.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && seaLineObject != null && other.name == seaLineObject.name)
        {
            if (inSea)
            {
                rigidBody.gravityScale = 0F;
                canMoveUp = true;
                OxygenBar.instance.SetBreathe(false);
            }
            else
            {
                rigidBody.gravityScale = 1F;
                canMoveUp = false;
                OxygenBar.instance.SetBreathe(true);
            }
        }

        if (other.name == "Water")
        {
            Color prevColor = other.GetComponent<SpriteRenderer>().color;
            other.GetComponent<SpriteRenderer>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 0.96f);
        }
    }

    /// <summary>
    /// Changes the player's ability to breathe and plays the underwater ambience.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.name == seaLineObject.name)
        {
            OxygenBar.instance.SetBreathe(true);

            if (transform.position.y > other.transform.position.y)
            {
                inSea = false;
            }
            else
            {
                inSea = true;
            }

            if (inSea)
            {
                seaAmbience.Pause();
                underWaterAmbience.enabled = true;
            }
            else
            {
                seaAmbience.UnPause();
                underWaterAmbience.enabled = false;
            }
        }

        if (other.CompareTag("Fish"))
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
            Vector2 dir = -(other.transform.position - transform.position);
            rigidBody.AddForce(dir * 50);
            if (!soundeffects)
                soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();

            int idx = UnityEngine.Random.Range(0, painsounds.Length);
            soundeffects.PlayOneShot(painsounds[idx]);
        }

        if (other.CompareTag("Tentacle") && damageTimerBoss <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
            if (!soundeffects)
                soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();

            int idx = UnityEngine.Random.Range(0, painsounds.Length);
            soundeffects.PlayOneShot(painsounds[idx]);
            damageTimerBoss = 1.0f;
        }
    }

    /// <summary>
    /// Plays the splash sound when the player enters the sea and picks up items.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inSea)
        {
            audioSource.PlayOneShot(splashSound);
            inSea = true;
        }

        if (other.CompareTag("Item"))
        {
            InventoryManager.instance.AddItems(other.GetComponent<DroppedItem>().item);
            audioSource.PlayOneShot(itemPickupSound);
            Destroy(other.gameObject);
        }

        if (other.name == "Water")
        {
            Color prevColor = other.GetComponent<SpriteRenderer>().color;
            other.GetComponent<SpriteRenderer>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 0.1f);
        }
    }

    /// <summary>
    /// Relinks the player's attributes if they are missing.
    /// </summary>
    public void RelinkAttributes()
    {
        sceneChanged = true;

        if (seaTopBoxCollider == null)
        {
            seaTopBoxCollider = GameObject.Find("SeaTopBox").GetComponent<Collider2D>();
            sceneChanged = false;
        }
        if (seaLineObject == null)
        {
            seaLineObject = GameObject.Find("seaLine");
            sceneChanged = false;
        }
        if (audioSource == null)
        {
            audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            sceneChanged = false;
        }
        if (seaAmbience == null)
        {
            seaAmbience = GameObject.Find("seaAmbience").GetComponent<AudioSource>();
            sceneChanged = false;
        }
        if (underWaterAmbience == null)
        {
            underWaterAmbience = GameObject.Find("underWaterAmbience").GetComponent<AudioSource>();
            sceneChanged = false;
        }
        if (underWaterMusic == null)
        {
            underWaterMusic = GameObject.Find("underWaterMusic").GetComponent<AudioSource>();
            sceneChanged = false;
        }

        if (seaTopBoxCollider != null && !Physics2D.GetIgnoreCollision(seaTopBoxCollider, playerCollider))
        {
            Physics2D.IgnoreCollision(seaTopBoxCollider, playerCollider, true);
            sceneChanged = false;
        }
    }

    // Getters and setters
    public bool isInSea()
    {
        return inSea;
    }

    public void stopMovement()
    {
        moveSpeed = 0;
    }
    public void startMovement()
    {
        moveSpeed = 5f;
    }
}
