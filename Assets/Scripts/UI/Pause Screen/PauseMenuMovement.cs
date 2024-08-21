using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PauseMenuMovement : MonoBehaviour
{
    //Variables
    public GameObject Camera;
    public GameObject PauseMenuContent;
    private Vector2 Target;
    private Vector2 CameraTarget;
    private Vector2 AboveCameraTarget;
    private Vector2 CurrentCameraPosition;
    private Vector2 CameraOffset = new Vector2(-1.9f, 2.6f);
    public float Velocity;
    public bool Paused;

    void Start()
    {
        Target = new Vector2(-1.9f, 18);
    }

    void Update()
    {
        CurrentCameraPosition = Camera.transform.position;
        CameraTarget = new Vector2(
            CurrentCameraPosition.x + CameraOffset.x,
            CurrentCameraPosition.y + CameraOffset.y
        );
        AboveCameraTarget = new Vector2(CameraTarget.x, CameraTarget.y + 15.4f);
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

        if (transform.position.y >= CameraTarget.y + 14)
        {
            PauseMenuContent.SetActive(false);
        }
        else
        {
            PauseMenuContent.SetActive(true);
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        Target = AboveCameraTarget;
        Paused = false;
    }

    public void PauseGame()
    {
        transform.position = AboveCameraTarget;
        Time.timeScale = 0f;
        Target = CameraTarget;
        Paused = true;
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Move()
    {
        transform.position = Vector2.Lerp(
            transform.position,
            Target,
            Velocity * Time.unscaledDeltaTime
        );
    }
}
