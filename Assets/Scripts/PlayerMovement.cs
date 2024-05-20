using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

    }

    void Move()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        
    }

}
