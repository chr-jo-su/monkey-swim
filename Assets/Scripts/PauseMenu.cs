using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public bool Paused;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuObject.SetActive(false);
    }

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
    }

    public void PauseGame()
    {
        PauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void UnpauseGame()
    {
        PauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void ReturnToTitle() 
    {
        Debug.Log("Returning to title...");
        // Add pathway to main title when main title page is created
    }
}
