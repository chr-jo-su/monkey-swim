using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color base_color = GetComponent<MeshRenderer>().material.color;
            base_color.a = 0.3f;
            GetComponent<MeshRenderer>().material.color = base_color;
        }
    }
}
