using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bananarang : MonoBehaviour
{
    public GameObject BananarangObject;
    private Vector3 Direction;
    private Vector3 MousePosition;
    private Vector3 PlayerPosition;
    private Camera MainCamera;
    public float Lifespan;
    public float Velocity;
    private float Timer;
    private float Rotation;
    public bool BananarangReady;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;
        Rotation = Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, Rotation);

        if (!BananarangReady)
        {
            Timer += Time.deltaTime;
            if (Timer >= Lifespan)
            {
                BananarangReady = true;
                Timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && BananarangReady)
        {
            BananarangReady = false;
            Instantiate(BananarangObject, BananarangObject.transform.position, Quaternion.identity);
        }
    }
}
