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
            BananarangStatic.SetActive(false);
            Timer += Time.deltaTime;
            if (Timer >= Lifespan)
            {
                BananarangClones = GameObject.FindGameObjectsWithTag("BananarangClone");
                foreach(GameObject obj in BananarangClones) {
                    Destroy(obj);
                }
                BananarangStatic.SetActive(true);
                BananarangReady = true;
                Timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && BananarangReady)
        {
            BananarangReady = false;
            Instantiate(BananarangObject, BananarangStatic.transform.position, Quaternion.identity);
        }
    }
}
