using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

    private bool inSea = false;
    private bool canMoveUp = true;

    public GameObject oxygenSlider;

    private float maxOxygen = 100.0f;

    private float oxygen = 100.0f;

    public float oxygenDepletionRate = 1.0f;

    public float oxygenGainRate = 1.0f;

    private bool canBreath = true;

    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip itemPickupSound;
    public AudioSource seaAmbience;
    public AudioSource underWaterAmbience;
    public AudioSource underWaterMusic;
    public Collider2D seaTopBoxCollider;
    public Collider2D playerCollider;
    public HealthBar playerHealth;
    private int drownTimer = 0;

    // Awake is called when the script instance is being loaded
    private void Awake() {
        instance = this;
    }

    public void TakeDamage(int damage) {
        playerHealth.TakeDamage(damage);
    }

    void Start() {
        Physics2D.IgnoreCollision(seaTopBoxCollider, playerCollider, true);
    }

    // Update is called once per frame
    void Update() {
        ProcessInputs();

        if (oxygen > 0.0f && !canBreath) {
            oxygen -= oxygenDepletionRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }
        else if (oxygen < maxOxygen && canBreath) {
            oxygen += 10 * oxygenGainRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }

        if (oxygen <= 0.0f) {
            // Debug.Log("monke painfully drowned :(");
            if (drownTimer == 200) {
                playerHealth.TakeDamage(1);
                drownTimer = 0;
            }
            drownTimer++;
        }
    }

    void FixedUpdate() {
        Move();
    }

    void ProcessInputs() {
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

    void Move() {
        //rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        Vector2 v = new Vector2(movement.x, movement.y);

        if (canMoveUp)
            v = new Vector2(movement.x, movement.y);
        else
            v = new Vector2(movement.x, 0);

        rigidBody.AddForce(v.normalized * moveSpeed);
    }

    public void ChangeOxygen(int val) {
        maxOxygen += val;

        if (val < 0)
        {
            oxygen = Math.Min(oxygen, maxOxygen);
        }
    }

    public void ChangeMoveSpeed(int val) {
        moveSpeed += val;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.name == seaLineObject.name) {
            if (inSea) {
                rigidBody.gravityScale = 0F;
                canMoveUp = true;
                canBreath = false;
            }
            else {
                rigidBody.gravityScale = 1F;
                canMoveUp = false;
                canBreath = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.name == seaLineObject.name) {
            canBreath = true;
            if (transform.position.y > other.transform.position.y)
                inSea = false;
            else
                inSea = true;

            if (inSea) {
                seaAmbience.Pause();
                underWaterAmbience.enabled = true;
            }
            else {
                seaAmbience.UnPause();
                underWaterAmbience.enabled = false;
            }

            //Debug.Log("inSea = " + inSea);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!inSea)
            audioSource.PlayOneShot(splashSound);

        if (other.CompareTag("Item")) {
            Debug.Log("Picked up " + other.name);
            // inventorySystem.GetComponent<InventoryManager>().AddItems(other.GetComponent<DroppedItem>().item);
            InventoryManager.instance.AddItems(other.GetComponent<DroppedItem>().item);
            audioSource.PlayOneShot(itemPickupSound);
            Destroy(other.gameObject);
        }
    }
}
