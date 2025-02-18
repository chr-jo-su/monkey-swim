using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    public GameObject BananarangObject;
    public GameObject[] BananarangClones;
    private Transform PlayerTransform;
    private Transform SealineTransform;
    private Vector3 BananarangPosition;
    private Vector3 Direction;
    private Vector3 MousePosition;
    private Camera MainCamera;
    public float MaxBananarangCount;
    public float BananarangCount;
    public bool WeaponRebound;
    private float Timer;
    private float Rotation;
    private bool BananarangReady;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SealineTransform = GameObject.FindGameObjectWithTag("Sealine").GetComponent<Transform>();
        BananarangReady = true;
        BananarangCount = 3;
    }

    void Update()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        BananarangPosition = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, 1);
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;
        //Rotation = Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, Rotation);

        if (BananarangCount < MaxBananarangCount)
        {
            Timer += Time.deltaTime;
            if (Timer >= 1)
            {
                BananarangCount += 1;
                Timer = 0;
            }
        }

        if (PlayerTransform.position.y < SealineTransform.position.y)
        {
            if (BananarangReady)
            {
                if (Input.GetMouseButton(0) && BananarangCount >= 1)
                {
                    Instantiate(BananarangObject, BananarangPosition, Quaternion.identity);
                    BananarangReady = false;
                    BananarangCount -= 1;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            BananarangReady = true;
        }
    }
}
