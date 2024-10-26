using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidAttack : MonoBehaviour
{
   //Variables
    // public Collider2D player;
    public const float moveSpeed = 4f;
    public Rigidbody2D rb;
    public GameObject gameObject;
    private Transform player;
    Vector2 movement;
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
        movement.x = 1;
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position - movement*moveSpeed*Time.fixedDeltaTime);

        if (transform.position.x <= -12)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
