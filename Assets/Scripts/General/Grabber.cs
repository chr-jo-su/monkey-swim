using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private float velocity = 2f;
    public bool goGrabbaGrabba;

    // Update is called once per frame
    void Update()
    {
        if (goGrabbaGrabba)
        {
            Grab();
        }
    }

    void Grab()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, Player.transform.position + new Vector3(0, 3.5f, 0), velocity * Time.unscaledDeltaTime);
    }


}
