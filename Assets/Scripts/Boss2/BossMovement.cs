using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossMovement : MonoBehaviour
{
    // public static BossMovement instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    // Start is called before the first frame update
    public PlayerMovement player;
    public int oxygenLoss = -15;
    public float speed = 0.0f;
    private bool isJumping = true;
    private bool facingRight = false;
    public Rigidbody2D rb;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (isJumping)
        {
            if (facingRight)
            {
                Debug.Log("JUMP!");
                rb.velocity = new Vector2(5 * GetForceDivide(), Mathf.Abs(transform.position.x - 1));
            }
            else
            {
                rb.velocity = new Vector2(-(Mathf.Abs(transform.position.x - 5)), 5 * GetForceDivide());
            }
        }
    }

    private float GetForceDivide()
    {
        float aiY = transform.position.y;
        float destinationY = 2;
        float test = Mathf.Abs(aiY - destinationY);
        return aiY > destinationY ? 1f / test : 1f;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OxygenBar.instance.ChangeOxygen(oxygenLoss);
            PlayerMovement.instance.stopMovement();
            yield return new WaitForSeconds(3);
            PlayerMovement.instance.startMovement();
            // Destroy(gameObject);

        }
    }
}
