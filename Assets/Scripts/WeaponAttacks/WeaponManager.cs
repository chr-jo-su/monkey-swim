using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject BananarangStatic;
    public GameObject BananarangObject;
    public GameObject[] BananarangClones;
    private Vector3 Direction;
    private Vector3 MousePosition;
    private Camera MainCamera;
    public float MaxBananarangCount;
    public float BananarangCount;
    public float Velocity;
    public float Lifespan;
    private float Timer;
    private float Rotation;
    private bool BananarangReady;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        BananarangReady = true;
        BananarangCount = 3;
    }

    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;
        Rotation = Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, Rotation);

        if (BananarangCount < MaxBananarangCount)
        {
            Timer += Time.deltaTime;
            if (Timer >= 1)
            {
                BananarangStatic.SetActive(true);
                BananarangCount += 1;
                Timer = 0;
            }
        }

        if (BananarangReady)
        {
            if (Input.GetMouseButton(0) && BananarangCount >= 1)
            {
                Instantiate(
                    BananarangObject,
                    BananarangStatic.transform.position,
                    Quaternion.identity
                );
                BananarangReady = false;
                BananarangCount -= 1;
                BananarangStatic.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            BananarangReady = true;
        }

        Debug.Log(BananarangCount);
    }
}
