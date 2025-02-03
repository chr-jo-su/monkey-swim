using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementAndOxygen : MonoBehaviour
{
    // Variables
    public static PlayerMovementAndOxygen instance;

    public float moveSpeed = 5f;
    public Rigidbody2D rigidBody;
    public GameObject seaLineObject; // should be a thin object with a boxCollider2D component and trigger enabled
                                     // that signifies the sea line
    Vector2 movement;
    public Animator animator;

    [HideInInspector] public bool sceneChanged = true;
    private bool inSea = false;
    private bool canMoveUp = true;
    private bool canBreath = true;

    public GameObject oxygenSlider;

    private float oxygen = 100.0f;
    private float maxOxygen = 100.0f;
    public float oxygenDepletionRate = 1.0f;
    public float oxygenGainRate = 1.0f;
    public int oxygenDepletionDamage = 3;
    public int drownTimer = 100;
    private int currentDrownTimer = 0;

    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip itemPickupSound;
    public AudioSource seaAmbience;
    public AudioSource underWaterAmbience;
    public AudioSource underWaterMusic;
    public Collider2D seaTopBoxCollider;
    public Collider2D playerCollider;

    // Awake is called when the script instance is being loaded
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

    // Update is called once per frame
    void Update()
    {
        if (sceneChanged)
        {
            RelinkAttributes();
        }

        ProcessInputs();

        if (oxygen > 0.0f && !canBreath)
        {
            oxygen -= oxygenDepletionRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }
        else if (oxygen < maxOxygen && canBreath)
        {
            oxygen += 10 * oxygenGainRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }

        if (oxygen <= 0.0f)
        {
            if (currentDrownTimer == drownTimer)
            {
                PlayerHealthBar.instance.TakeDamage(oxygenDepletionDamage);
                currentDrownTimer = 0;
            }
            currentDrownTimer++;
        }
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
        // if (moveX == 1) {
        //     GetComponent<SpriteRenderer>().flipX = true;
        // }
        // else {
        //     GetComponent<SpriteRenderer>().flipX = false;
        // }
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
    /// Changes the oxygen level of the player by the given value.
    /// </summary>
    /// <param name="val">The value to change the oxygen level by. Can be negative.</param>
    public void ChangeOxygen(int val)
    {
        maxOxygen += val;

        if (val < 0)
        {
            oxygen = Math.Min(oxygen, maxOxygen);
        }
    }

    /// <summary>
    /// Changes the move speed of the player by the given value.
    /// </summary>
    /// <param name="val">The value to change the move speed by. Can be negative.</param>
    public void ChangeMoveSpeed(int val)
    {
        moveSpeed += val;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.name == seaLineObject.name)
        {
            if (inSea)
            {
                rigidBody.gravityScale = 0F;
                canMoveUp = true;
                canBreath = false;
            }
            else
            {
                rigidBody.gravityScale = 1F;
                canMoveUp = false;
                canBreath = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.name == seaLineObject.name)
        {
            canBreath = true;
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
    }

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
}
