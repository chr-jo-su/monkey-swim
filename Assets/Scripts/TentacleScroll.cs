using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleScroll : MonoBehaviour
{
   //Variables
    public const float moveSpeed = 2.5f;
    public Rigidbody2D rb;
    public GameObject gameObject;
    private Transform player;
    Vector2 movement;
    public bool moving = true;
    public int direction = 1;
    public HealthBar playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        movement.y = 1;
    }

    void FixedUpdate()
    {
        //Movement
        if (moving)
        {
            rb.MovePosition(rb.position + direction*movement*moveSpeed*Time.fixedDeltaTime);
        }

        if (transform.position.y >= 0)
        {
            for (int i = 0; i < 100; i++)
            {
                moving = false;
            }
            moving = true;
            direction = -1;
        }

        if (transform.position.y <= -15)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(10);
            // following = false;
            // attackCounter = 0;
            Debug.Log("Take Damage");
        }
    }
}
