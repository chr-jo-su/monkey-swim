using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class Bananarang : MonoBehaviour
{
    private UnityEngine.Vector3 MousePosition;
    private UnityEngine.Vector3 Direction;
    private Camera MainCamera;
    private Rigidbody2D RigidBody;
    public float Velocity;
    private float Angle;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RigidBody = GetComponent<Rigidbody2D>();
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;
        RigidBody.velocity =
            new UnityEngine.Vector2(Direction.x, Direction.y).normalized * Velocity;
    }

    // Update is called once per frame
    void Update()
    {
        Angle += 0.0008f;
        transform.rotation = quaternion.Euler(0, 0, Angle * Mathf.Rad2Deg);
    }
}
