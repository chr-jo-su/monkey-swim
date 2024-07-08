using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 5f;
    public Rigidbody2D rigidBody;
    public GameObject seaLineObject; // should be a thin object with a boxCollider2D component and trigger enabled
                                     // that signifies the sea line
    Vector2 movement;
    public Animator animator;

    private bool inSea = false;
    private bool canMoveUp = true;

    public GameObject oxygenSlider;

    private float oxygen = 100.0f;

    public float oxygenDepletionRate = 1.0f;

    public float oxygenGainRate = 1.0f;

    private bool canBreath = true;
    
    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioSource seaAmbience;
    public AudioSource underWaterAmbience;
    public AudioSource underWaterMusic;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        if (oxygen > 0.0f && !canBreath)
        {
            oxygen -= oxygenDepletionRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen*0.01f;
        }
        else if (oxygen < 100.0f && canBreath)
        {
            oxygen += oxygenGainRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen*0.01f;
        }

        if (oxygen <= 0.0f)
        {
            Debug.Log("monke painfully drowned :(");
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(moveX));

        movement = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        //rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        Vector2 v = new Vector2(movement.x, movement.y);

        if (canMoveUp)
            v = new Vector2(movement.x, movement.y);
        else
            v = new Vector2(movement.x, 0);

        rigidBody.AddForce(v.normalized * moveSpeed);
    }
    
    void OnTriggerExit2D(Collider2D other)
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

    void OnTriggerStay2D(Collider2D other) {
        canBreath = true;
        
        if (other.name == seaLineObject.name)
        {
            if (transform.position.y > other.transform.position.y)
                inSea = false;
            else
                inSea = true;
            
            Debug.Log("inSea = " + inSea);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!inSea)
            audioSource.PlayOneShot(splashSound);
    }
}
