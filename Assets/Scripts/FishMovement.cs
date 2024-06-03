using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishMovement : MonoBehaviour
{
   //Variables
    public float moveSpeed = -5f;
    public Rigidbody2D rb;
    private float theta = 0f;
    private float thetaStep = Mathf.PI / 32f;
    Vector2 movement;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       //Input
       movement.x = 1;
       movement.y = Mathf.Sin(theta);
       // Allows for movement in the y axis to be a bit less linear, to imitate the swimming slightly better; could be changed though
    }

    void FixedUpdate()
    {
        theta += thetaStep; //*moveSpeed

        //Movement
        rb.MovePosition(rb.position + movement*moveSpeed*Time.fixedDeltaTime);
        transform.Rotate(Vector3.forward * Mathf.Sin(theta));


        //Change Directions; -8 and 8 are arbitrary edges of the screen; should be reworked later
        if ((transform.position.x >= 8 && moveSpeed > 0) | (transform.position.x <= -8 && moveSpeed < 0))
        {
            moveSpeed *= -1;
            gameObject.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

    }

}
