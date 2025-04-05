using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FishMovementOLD : MonoBehaviour
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
    private EnemyHealthBar fishHealth;
    private Vector2 spawnPos;

    void Start()
    {
        healthMax = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthSystem = transform.Find("Canvas/EnemyHealthManager").gameObject;
        fishHealth = gameObject.GetComponentInChildren<EnemyHealthBar>();
        spawnPos = this.transform.position;
        seaLineObject = GameObject.Find("seaLine");
    }

    void Update()
    {
        //Input
        movement.x = 1;
        movement.y = Mathf.Sin(theta) * 0.4f;

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

        if (following && attackCounter >= 11)
        {
            transform.position = Vector2.MoveTowards(
                this.transform.position,
                player.transform.position,
                moveSpeed * Time.deltaTime / 2
            );

            var dir = player.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (distance >= 4)
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
            } else if (theta <= 0)
            {
                theta = 2 * Mathf.PI;
            }

            //Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            //transform.Rotate(Vector3.forward * Mathf.Sin(theta));
            if (movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle * 0.5f, Vector3.forward);
            }

            //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
            if (
                (transform.position.x >= (spawnPos.x + 8) && moveSpeed > 0)
                || (transform.position.x <= (spawnPos.x - 8) && moveSpeed < 0)
            )
            {
                moveSpeed *= -1;
                thetaStep = -thetaStep;
                //gameObject.transform.localScale = new Vector2(
                //    -transform.localScale.x,
                //    transform.localScale.y
                //);
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
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
        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            PlayerHealthBar.instance.TakeDamage(damage);
            following = false;
            attackCounter = 0;
        }

        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            health -= damageTaken;
            fishHealth.TakeDamage(BananarangDamage.instance.GetDamage());
        }

        // yikes
        if (collision.gameObject.name.Contains("Square")
                || collision.gameObject.name.Contains("Triangle")
                || collision.gameObject.name.Contains("Hexagon")
                || collision.gameObject.name.Contains("seaLine"))
        {
            moveSpeed *= -1;
            thetaStep = -thetaStep;
            //gameObject.transform.localScale = new Vector2(
            //    -transform.localScale.x,
            //    transform.localScale.y
            //);
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }

        if (collision.gameObject.tag == "Fish")
        {

            if (GetComponent<PolygonCollider2D>())
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<PolygonCollider2D>());
            } else
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    { // not working correctly
        //if (other.name == seaLineObject.name)
        //{
        //    if (transform.position.y >= other.transform.position.y + 100)
        //    {
        //        movement.y *= movement.y;
        //    }


        //}

        //moveSpeed *= -1;
        //gameObject.transform.localScale = new Vector2(
        //    -transform.localScale.x,
        //    transform.localScale.y
        //);
    }
}
