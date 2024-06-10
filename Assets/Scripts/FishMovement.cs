using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class fishMovement : MonoBehaviour
{
   //Variables
    public float moveSpeed = -5f;
    public Rigidbody2D rigidBody;
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
        rigidBody.MovePosition(rigidBody.position + movement*moveSpeed*Time.fixedDeltaTime);

        //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
        if (transform.position.x <= -8 && moveSpeed < 0) {
            moveSpeed *= -1;
        }

        if (transform.position.x >= 8 && moveSpeed > 0) {
            moveSpeed *= -1;
        }

    }

}
