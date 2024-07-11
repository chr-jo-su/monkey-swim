using UnityEngine;
using UnityEngine.EventSystems;

public class ContineClick : MonoBehaviour, IPointerClickHandler
{

    public PauseMenuMovement menu;
    // public Collider collide;

    void Awake()
    {
        // if (collide.isTrigger)
        // {
        //     collide.isTrigger = false;
        // }
        // else
        // {
        //     collide.isTrigger = true;
        // }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // collide.isTrigger = true;
        Debug.Log("Clicking on Words");
        menu.UnpauseGame();
    }
}
