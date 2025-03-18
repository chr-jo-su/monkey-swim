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

    /// <summary>
    /// Creates a singleton instance of the PlayerMovement.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Removes health from the player.
    /// </summary>
    /// <param name="damage">The amount of health to remove from the player.</param>
    public void TakeDamage(int damage)
    {
        PlayerHealthBar.instance.TakeDamage(damage);
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
    }

    void FixedUpdate()
    {
        Move();
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
        }

        if (other.CompareTag("Item"))
        {
            InventoryManager.instance.AddItems(other.GetComponent<DroppedItem>().item);
            audioSource.PlayOneShot(itemPickupSound);
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Relinks the player's attributes if they are missing.
    /// </summary>
    public void RelinkAttributes()
    {
        if (seaTopBoxCollider == null)
        {
            seaTopBoxCollider = GameObject.Find("SeaTopBox").GetComponent<Collider2D>();
        }
        if (seaLineObject == null)
        {
            seaLineObject = GameObject.Find("seaLine");
        }
        if (audioSource == null)
        {
            audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        }
        if (seaAmbience == null)
        {
            seaAmbience = GameObject.Find("seaAmbience").GetComponent<AudioSource>();
        }
        if (underWaterAmbience == null)
        {
            underWaterAmbience = GameObject.Find("underWaterAmbience").GetComponent<AudioSource>();
        }
        if (underWaterMusic == null)
        {
            underWaterMusic = GameObject.Find("underWaterMusic").GetComponent<AudioSource>();
        }

        if (seaTopBoxCollider != null && !Physics2D.GetIgnoreCollision(seaTopBoxCollider, playerCollider))
        {
            sceneChanged = false;
            Physics2D.IgnoreCollision(seaTopBoxCollider, playerCollider, true);
        }
    }

    // Getters and setters
    public bool isInSea()
    {
        return inSea;
    }

    public void stopMovement() {
        moveSpeed = 0;
    }
}
