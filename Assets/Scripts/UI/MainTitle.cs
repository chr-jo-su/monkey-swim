using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject ChimpButton;
    public float Velocity;
    public bool MrPresidentTheMonkeyHasBeenPressed = false;

    void Update() {
        if (MrPresidentTheMonkeyHasBeenPressed == true) {
            HideMainTitleMenu();
        }
        if (transform.position.y > 10) {
            Menu.SetActive(false);
        }
    }

    public void HideMainTitleMenu() {
        transform.position = Vector2.Lerp(transform.position, new Vector2(0,30), Velocity * Time.unscaledDeltaTime);
    }

    public void SecretServiceAnnouncementToThePresident() {
        MrPresidentTheMonkeyHasBeenPressed = true;
    }

}
