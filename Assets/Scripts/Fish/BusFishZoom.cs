using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusFishZoom : MonoBehaviour
{
    private Transform busFishTransform;
    private new Camera camera;
    private int zoomStartVerticalLevel;
    private int zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        zoomSpeed = 5;
        busFishTransform = GameObject.Find("BusFish").transform;
        zoomStartVerticalLevel = (int)(busFishTransform.position.y * 0.9f);
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= zoomStartVerticalLevel) {
            camera.orthographicSize += zoomSpeed * Time.unscaledDeltaTime;
        } else {
            camera.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
        }
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 6f, 10f);
    }
}
