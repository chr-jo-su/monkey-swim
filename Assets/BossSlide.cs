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
    public float health = 500.0f;
    public HealthBar bossHealth;
    private Rigidbody2D rb;
    private Vector3 StartPosition;
    private float DistanceOut = 18;
    private float SlideSpeed = 15;
    public bool SlideOut = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        TentacleManager.instance.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        BossSlideOut();
        BossSlideIn();
    }

    public void BossSlideIn()
    {
        if (SlideOut == false)
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
        if (SlideOut == true)
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
