using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PauseMenuMovement : MonoBehaviour
{
    //Variables
    public Transform TargetDown;
    public Transform TargetUp;
    public Transform Target;
    public float Velocity;
    public bool Paused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
        Move();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;        
        Target = TargetUp;
        // PauseMenu.SetActive(false);
        Paused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Target = TargetDown;
        // PauseMenu.SetActive(true);
        Paused = true;
    }

    void Move()
    {
        transform.position = Vector2.Lerp(transform.position, Target.position, Velocity * Time.unscaledDeltaTime);
    }
}

// Smooth towards the target

    // void Update() {
    //     transform.position = Vector2.Lerp(transform.position, target.position, Velocity * Time.deltaTime);
    // }
