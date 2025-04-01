using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToScreen : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject imageObject;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width / 1920f > Screen.height / 1080f)
        {
            imageObject.transform.localScale = new Vector3(Screen.height / 1080f, Screen.height / 1080f, 1);
        }
        else
        {
            imageObject.transform.localScale = new Vector3(Screen.width / 1920f, Screen.width / 1920f, 1);
        }
    }
}
