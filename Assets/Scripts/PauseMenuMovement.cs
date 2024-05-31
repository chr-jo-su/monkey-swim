using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PauseMenuMovement : MonoBehaviour
{
    //Variables
    public float MoveSpeed = 30f;
    public float StoppingDistance = 13f;
    public Rigidbody2D rb;
    Vector2 Movement;

    // Update is called once per frame
    void Update()
    {
        Movement = new Vector2(0, -1);
        Move();
    }

    void Move()
    {
        Vector2 v = new Vector2(Movement.x, Movement.y);
        if (gameObject.transform.position.y > StoppingDistance) {
            rb.AddForce(v.normalized * MoveSpeed);
        }
    }

}
