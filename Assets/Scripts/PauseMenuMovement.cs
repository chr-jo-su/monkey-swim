using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PauseMenuMovement : MonoBehaviour
{
    //Variables
    public float MoveSpeed = 30f;
    public Rigidbody2D rb;
    bool Paused;
    bool Moving;
    Vector2 Movement;

    void Start()
    {
        Movement = new Vector2(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                UnpauseGame();
                Vector2 v = new Vector2(Movement.x, -Movement.y);
                Move(v);
            }
            else
            {
                PauseGame();
                Vector2 v = new Vector2(Movement.x, Movement.y);
                Move(v);
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Paused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        Paused = false;
    }

    void Move(Vector2 v)
    {
        rb.transform.Translate(v * MoveSpeed);
    }
}
