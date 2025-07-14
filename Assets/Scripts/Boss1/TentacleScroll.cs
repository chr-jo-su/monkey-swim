using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleScroll : MonoBehaviour
{
   //Variables
    public const float moveSpeed = 2.5f;
    public Rigidbody2D rb;
    private Transform player;
    Vector2 movement;
    public bool moving = true;
    public int direction = 1;
    private int damage = 10;

    private float damageTimer = 0.0f;

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

        if (transform.position.y >= -2)
        {
            for (int i = 0; i < 100; i++)
            {
                moving = false;
            }
            moving = true;
            direction = -1;
        }

        if (transform.position.y <= -16)
        {
            TentacleManager.instance.enabled = false;
            QuidManager.instance.enabled = true;
            BossSlide.instance.SlideIn = false;
            Destroy(gameObject);
        }

        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    /// <summary>
    /// If the player collides with the enemy, it gets the instane of the health
    /// and deals damage
    /// </summary>
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && damageTimer <= 0)
        {
            PlayerHealthBar.instance.TakeDamage(damage);
            damageTimer = 1;
        }
    }
}
