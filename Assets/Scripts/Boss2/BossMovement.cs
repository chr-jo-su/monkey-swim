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
    public int directionY = 5;
    public int directionX = -2;
    public float moveSpeed = 5f;
    public float acc = 0f;
    Vector2 v;
    Vector2 accV;


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

        if (transform.position.y >= 5.5f)
        {
            directionY = -2;
            moving = true;
            acc = 1;
        }
        else if (transform.position.x <= -11)
        {
            Debug.Log("switch");
            moving = true;
            directionX = 1;
        }
        else if (transform.position.x >= 10)
        {
            moving = true;
            directionX = -2;
        }
        else if (transform.position.y <= -4.5)
        {
            directionY = 10;
            moving = true;
            acc = -14.7f; //-14.7
        }


        v = new Vector2(directionX, directionY);
        accV = new Vector2(1, acc);
        rb.MovePosition(rb.position + v * moveSpeed * Time.fixedDeltaTime + accV*Time.fixedDeltaTime*Time.fixedDeltaTime);

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
