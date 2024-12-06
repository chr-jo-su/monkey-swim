using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FishHealthBarPosition : MonoBehaviour
{
    private GameObject ParentFish;
    private Vector3 HealthBarPosition;
    private Vector3 FishPosition;

    void Start()
    {
        ParentFish = transform.root.gameObject;
    }

    void Update()
    {
        FishPosition = ParentFish.transform.position;

        transform.position = new Vector3(FishPosition.x, FishPosition.y + 0.75f, 2);
        transform.rotation = Quaternion.identity;
    }
}
