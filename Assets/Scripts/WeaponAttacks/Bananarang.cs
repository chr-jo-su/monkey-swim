using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bananarang : MonoBehaviour
{
    private UnityEngine.Vector3 PlayerPosition;
    private UnityEngine.Vector3 MousePosition;
    private UnityEngine.Vector3 Direction;
    private Rigidbody2D RigidBody;
    private Collider2D BananarangCollider;
    private Collider2D PlayerCollider;
    private Camera MainCamera;
    private float Angle;
    private float Timer;
    public float Velocity;
    private bool ReturnToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        PlayerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        BananarangCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(BananarangCollider, PlayerCollider, true);
        RigidBody = GetComponent<Rigidbody2D>();
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;
        RigidBody.velocity =
            new UnityEngine.Vector2(Direction.x, Direction.y).normalized * Velocity;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Angle += 0.0008f;
        transform.rotation = quaternion.Euler(0, 0, Angle * Mathf.Rad2Deg);

        Timer += Time.deltaTime;
        if (Timer >= 1)
        {
            ReturnToPlayer = true;
        }
        if (ReturnToPlayer == true)
        {
            Direction = PlayerPosition - transform.position;
            RigidBody.velocity =
                new UnityEngine.Vector2(Direction.x, Direction.y).normalized * Velocity;
            if (UnityEngine.Vector2.Distance(transform.position, PlayerPosition) < 1) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        ReturnToPlayer = true;
    }
}
