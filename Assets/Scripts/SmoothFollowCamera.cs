using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public GameObject playerObject;

    // Update is called once per frame
    void Update()
    {   
        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
