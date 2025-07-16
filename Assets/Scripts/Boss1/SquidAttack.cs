using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidAttack : MonoBehaviour
{
    // Variables
    public const float moveSpeed = 4f;
    public Rigidbody2D rb;
    private int damage = 20;
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(
            gameObject.GetComponent<Collider2D>(),
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Collider2D>(),
            true
        );

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = 1;
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position - movement * moveSpeed * Time.fixedDeltaTime);

        if (transform.position.x <= -16)
        {
            SquidManager.instance.enabled = false;
            TentacleManager.instance.enabled = true;
            BossSlide.instance.slideIn = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// If the player collides with the enemy, it gets the instance of the health and deals damage.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthBar.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
