using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossMovement : MonoBehaviour
{
    // public static BossMovement instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    // Start is called before the first frame update
    public PlayerMovement player;
    public int oxygenLoss = -15;
    public float speed = 0.0f;
    // private bool isJumping = true;
    // private bool facingRight = false;
    public Rigidbody2D rb;
    public bool moving = true;
    public int directionY = 1;
    public int directionX = -1;
    public float moveSpeed = 2.5f;
    Vector2 v;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        v = new Vector2(directionX, directionY);
    }

    private void Update()
    {
        // if (moving)
        // {
        //     rb.MovePosition(rb.position + direction*movement*moveSpeed*Time.fixedDeltaTime);
        // }

        if (transform.position.y >= 7)
        {
            moving = true;
            directionY = -1;
        }

        if (transform.position.x <= -11)
        {
            Debug.Log("AHHHHH");
            moving = true;
            directionX = 1;
        }
        if (transform.position.x >= 11)
        {
            moving = true;
            directionX = -2;
        }

        if (transform.position.y <= -2)
        {
            Debug.Log("AHHHHH");
            moving = true;
            directionY = 1;
        }


        v = new Vector2(directionX, directionY);

        rb.AddForce(v.normalized * moveSpeed);

    }

    // private float GetForceDivide()
    // {
    //     float aiY = transform.position.y;
    //     float destinationY = 2;
    //     float test = Mathf.Abs(aiY - destinationY);
    //     return aiY > destinationY ? 1f / test : 1f;
    // }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OxygenBar.instance.ChangeOxygen(oxygenLoss);
            PlayerMovement.instance.stopMovement();
            yield return new WaitForSeconds(3);
            PlayerMovement.instance.startMovement();
            // Destroy(gameObject);

        }
    }
}
