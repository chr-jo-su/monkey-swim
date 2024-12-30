using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class BossSlide : MonoBehaviour
{
    public static BossSlide instance;
    public float health = 500.0f;
    public HealthBar bossHealth;
    private Rigidbody2D rb;
    private Vector3 StartPosition;
    private float DistanceOut = 145;
    private float SlideSpeed = 15;
    public bool SlideIn = false;
    public TentacleManager tent;
    public QuidManager quid;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        tent.enabled = true;
        quid.enabled = false;
        SlideIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        BossSlideOut();
        BossSlideIn();
        if (bossHealth.GetHealth() < bossHealth.GetMaxHealth()/2) {
            tent.enabled = false;
            quid.enabled = true;
            // SlideIn = false;
        }
    }

    public void BossSlideIn()
    {
        if (SlideIn == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                StartPosition,
                SlideSpeed * Time.unscaledDeltaTime
            );
        }
    }

    public void BossSlideOut()
    {
        if (SlideIn == true)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                new Vector3(DistanceOut, StartPosition.y, StartPosition.z),
                SlideSpeed * Time.unscaledDeltaTime
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            bossHealth.TakeDamage(25);
        }
    }
}
