using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusFishZoom : MonoBehaviour
{
    private Transform busFishTransform;
    private Camera cameraObject;
    private int zoomStartVerticalLevel;
    private int zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        zoomSpeed = 5;
        busFishTransform = GameObject.Find("BusFish").transform;
        zoomStartVerticalLevel = (int)(busFishTransform.position.y * 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (transform.position.y <= zoomStartVerticalLevel) {
            cameraObject.orthographicSize += zoomSpeed * Time.unscaledDeltaTime;
        } else {
            cameraObject.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
        }
        cameraObject.orthographicSize = Mathf.Clamp(cameraObject.orthographicSize, 6f, 10f);
    }
}
