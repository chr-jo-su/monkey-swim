using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    private bool inSea = false;
    private bool canMoveUp = true;

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
        //rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        Vector2 v = new Vector2(movement.x, movement.y);

        if (canMoveUp)
            v = new Vector2(movement.x, movement.y);
        else
            v = new Vector2(movement.x, 0);

        rb.AddForce(v.normalized * moveSpeed);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "sea")
        {
            if (transform.position.y > other.transform.position.y)
                inSea = false;
            else
                inSea = true;
            
            Debug.Log("inSea = " + inSea);
        }

        if (inSea)
        {
            rb.gravityScale = 0F;
            canMoveUp = true;
        }
        else
        {
            rb.gravityScale = 1F;
            canMoveUp = false;
        }
    }

}
