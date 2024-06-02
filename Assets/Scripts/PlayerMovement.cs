using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class playerMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 5f;
    public Rigidbody2D rigidBody;
    public GameObject seaLineObject; // should be a thin object with a boxCollider2D component and trigger enabled
                                     // that signifies the sea line
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

        rigidBody.AddForce(v.normalized * moveSpeed);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == seaLineObject.name)
        {
            if (transform.position.y > other.transform.position.y)
                inSea = false;
            else
                inSea = true;
            
            Debug.Log("inSea = " + inSea);
        }

        if (inSea)
        {
            rigidBody.gravityScale = 0F;
            canMoveUp = true;
        }
        else
        {
            rigidBody.gravityScale = 1F;
            canMoveUp = false;
        }
    }

}
