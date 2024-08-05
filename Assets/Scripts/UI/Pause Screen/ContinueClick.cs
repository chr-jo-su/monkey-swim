using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueClick : MonoBehaviour
{
    PauseMenuMovement PauseScript;

    void Start()
    {
        PauseScript = GameObject.FindGameObjectWithTag("PauseMenuManager").GetComponent<PauseMenuMovement>();
    }

    void OnMouseDown()
    {
        PauseScript.UnpauseGame();
    }
}
