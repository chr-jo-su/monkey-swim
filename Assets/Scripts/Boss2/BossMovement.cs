using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class BossMovement : MonoBehaviour
{
    public static BossMovement instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerMovement.instance.Suffocate();
            OxygenBar.instance.ChangeOxygen(-15);
        }
    }
}
