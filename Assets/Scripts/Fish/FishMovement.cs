using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class fishMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = -2.5f;
    public Rigidbody2D rb;
    private float theta = 0f;
    private float thetaStep = Mathf.PI / 32f;
    // public Rigidbody2D rigidBody;
    Vector2 movement;
    public bool following = false;
    public float distance;
    private Transform player;

    [SerializeField]
    public int damage;
    public int damageTaken;
    private int attackCounter = 0;
    public GameObject seaLineObject;
    private GameObject healthSystem;
    public float health = 100.0f;
    private float healthMax;
    public HealthBar fishHealth;

    void Start()
    {
        healthMax = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthSystem = transform.Find("Canvas/HealthSys").gameObject;
    }

    void Update()
    {
        //Input
        movement.x = 1;
        movement.y = Mathf.Sin(theta);
        // Allows for movement in the y axis to be a bit less linear, to imitate the swimming slightly better; could be changed though

        // Death
        if (health <= 0.0f)
        {
            GetComponent<ItemDropper>().enabled = true;
            if (GetComponent<ItemDropper>().finished)
                Destroy(this.gameObject);
        }

        if (health < healthMax)
        {
            healthSystem.transform.localScale = new Vector3(0.2f, 0.1f, 1);
        }

        // health -= 0.01f;
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        //Movement
        // rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (following && attackCounter >= 10)
        {
            transform.position = Vector2.MoveTowards(
                this.transform.position,
                player.transform.position,
                moveSpeed * Time.deltaTime / 2
            );

            if (distance >= 3)
            {
                following = false;
            }
        }
        else
        {
            theta += thetaStep; //*moveSpeed
            if (theta >= 2 * Mathf.PI)
            {
                theta = 0f;
            }

            //Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            transform.Rotate(Vector3.forward * Mathf.Sin(theta));

            //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
            if (
                (transform.position.x >= 8 && moveSpeed > 0)
                | (transform.position.x <= -8 && moveSpeed < 0)
            )
            {
                moveSpeed *= -1;
                gameObject.transform.localScale = new Vector2(
                    -transform.localScale.x,
                    transform.localScale.y
                );
            }

            if (distance < 3)
            {
                following = true;
            }
        }

        attackCounter++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovementAndOxygen>();
        if (player != null)
        {
            Debug.Log(damage);
            PlayerBar.instance.TakeDamage(damage);
            following = false;
            attackCounter = 0;
        }

        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            health -= damageTaken;
            fishHealth.TakeDamage(25);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    { // not working correctly
        if (other.name == seaLineObject.name)
        {
            if (transform.position.y >= other.transform.position.y + 100)
            {
                movement.y *= movement.y;
            }
        }
    }
}
