using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTarget : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
