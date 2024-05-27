using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishMovement : MonoBehaviour
{
   //Variables
    public float moveSpeed = -5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void Update()
    {
       //Input
       movement.x = 1;
       movement.y = (Random.Range(-.2f, .2f));
       // Allows for movement in the y axis to be a bit less linear, to imitate the swimming slightly better; could be changed though
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement*moveSpeed*Time.fixedDeltaTime);

        //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
        if (transform.position.x <= -8 && moveSpeed < 0) {
            moveSpeed *= -1;
        }

        if (transform.position.x >= 8 && moveSpeed > 0) {
            moveSpeed *= -1;
        }

    }

}
