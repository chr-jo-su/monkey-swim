using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject ChimpButton;
    public GameObject BigRedArrow;
    public float Velocity;
    public bool MrPresidentTheMonkeyHasBeenPressed = false;

    void Update()
    {
        if (MrPresidentTheMonkeyHasBeenPressed == true)
        {
            HideMainTitleMenu();
            BigRedArrow.SetActive(true);
        }
        else
        {
            BigRedArrow.SetActive(false);
        }

        if (transform.localPosition.y - Camera.main.pixelHeight >= Camera.main.pixelHeight * (2 / 3))
        {
            Menu.SetActive(false);
        }
    }

    public void HideMainTitleMenu()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(0, Screen.height * 1.5f), Velocity * Time.unscaledDeltaTime);
    }

    public void SecretServiceAnnouncementToThePresident()
    {
        MrPresidentTheMonkeyHasBeenPressed = true;
    }

}
