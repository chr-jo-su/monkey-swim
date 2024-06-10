using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject ChimpButton;
    public Transform Target;
    public float Velocity;
    public bool MrPresidentTheMonkeyHasBeenPressed = false;

    void Update() {
        if (MrPresidentTheMonkeyHasBeenPressed == true) {
            HideMainTitleMenu();
        }
    }

    public void HideMainTitleMenu() {
        transform.position = Vector2.Lerp(transform.position, Target.position, Velocity * Time.unscaledDeltaTime);
    }

    public void SecretServiceAnnouncementToThePresident() {
        MrPresidentTheMonkeyHasBeenPressed = true;
    }

}
