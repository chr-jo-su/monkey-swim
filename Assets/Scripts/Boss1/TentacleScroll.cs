using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleScroll : MonoBehaviour
{
    // Variables
    public const float moveSpeed = 2.5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public bool moving = true;
    public int direction = 1;
    private int damage = 10;

    private float damageTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.y = 1;
    }

    // Fixed Update is called at a set interval
    void FixedUpdate()
    {
        // Movement
        if (moving)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime * movement);
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
            SquidManager.instance.enabled = true;
            BossSlide.instance.slideIn = false;

            Destroy(gameObject);
        }

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// If the player collides with the enemy, it gets the instance of the health and deals damage.
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTimer <= 0)
        {
            PlayerHealthBar.instance.TakeDamage(damage);
            damageTimer = 1;
        }
    }
}
