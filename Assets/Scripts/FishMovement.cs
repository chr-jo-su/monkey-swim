using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class fishMovement : MonoBehaviour
{
   //Variables
    public float moveSpeed = -2.5f;
    public Rigidbody2D rb;
    private float theta = 0f;
    private float thetaStep = Mathf.PI / 32f;
    public Rigidbody2D rigidBody;
    Vector2 movement;
    public bool following = false;
    public float distance;
    private Transform player;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
       //Input
       movement.x = 1;
       movement.y = Mathf.Sin(theta);
       // Allows for movement in the y axis to be a bit less linear, to imitate the swimming slightly better; could be changed though
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        //Movement
        rigidBody.MovePosition(rigidBody.position + movement*moveSpeed*Time.fixedDeltaTime);

        if (following)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            
            if (distance > 3)
            {
                following = false;
            }
        }

        else
        {
            theta += thetaStep; //*moveSpeed
            if (theta >= 2*Mathf.PI)
            {
                theta = 0f;
            }

            //Movement
            rb.MovePosition(rb.position + movement*moveSpeed*Time.fixedDeltaTime);
            transform.Rotate(Vector3.forward * Mathf.Sin(theta));


            //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
            if ((transform.position.x >= 8 && moveSpeed > 0) | (transform.position.x <= -8 && moveSpeed < 0))
            {
                moveSpeed *= -1;
                gameObject.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }

            if (distance < 3)
            {
                following = true;
            }

        }
    }

    void OnCollisionEnter2D (Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") 
        {
            // currentHealth.TakeDamage(damage);
            Debug.Log("Player is hit");
        }
    }

}
