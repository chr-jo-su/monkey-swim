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
    public bool isJumping = true;
    public int jumpCount = 0;
    // private bool facingRight = false;
    public Rigidbody2D rb;
    public bool moving = true;
    public int directionY = 5;
    public int directionX = -2;
    public float moveSpeed = 5f;
    public float acc = 0f;
    Vector2 v;
    Vector2 accV;
    private Vector3 EndPosition;
    private float SlideSpeed = 15;
    private Vector3 StartPosition;
    public float DistanceOut = 8;
    public GameObject[] logs;
    public GameObject[] bossObjects;
    public int[] spawnPositionX;
    public int[] spawnPositionY;
    private int counter = 0;



    private void Start()
    {
        Debug.Log("start fnct");
        DeactivateAllLogs();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        StartPosition = gameObject.transform.position;
        EndPosition = new Vector3(StartPosition.x, StartPosition.y, StartPosition.z);
        v = new Vector2(directionX, directionY);
        // BeeManager.instance.TurnOff();
    }

    private void Update()
    {
        if (isJumping)
        {
            if (transform.position.y >= 5.5f)
            {
                directionY = -2;
                moving = true;
                acc = 1;
                jumpCount++;
            }
            else if (transform.position.x <= -11)
            {
                Debug.Log("switch");
                moving = true;
                directionX = 1;
                jumpCount++;
            }
            else if (transform.position.x >= 10)
            {
                moving = true;
                directionX = -2;
                jumpCount++;
            }
            else if (transform.position.y <= -4.5)
            {
                directionY = 10;
                moving = true;
                acc = -14.7f; //-14.7
                jumpCount++;
            }


            v = new Vector2(directionX, directionY);
            accV = new Vector2(1, acc);
            rb.MovePosition(rb.position + v * moveSpeed * Time.fixedDeltaTime + accV*Time.fixedDeltaTime*Time.fixedDeltaTime);

            if (jumpCount > 50)
            {
                isJumping = false;
                foreach (double s in spawnPositionX)
                {
                    if (counter < 3)
                    {
                        Vector2 spawnPos = new Vector2(spawnPositionX[counter], spawnPositionY[counter]);
                        Instantiate(bossObjects[counter], spawnPos, Quaternion.identity);
                    }
                    // Debug.Log(counter);
                    counter++;
                }
                counter = 0;
            }
        }
        else
        {
            StartSlideOut();
            if (GameObject.Find("bee(Clone)") == null)
            {
                Debug.Log("its time to jump");
                DeactivateAllLogs();
                jumpCount = 0;
                isJumping = true;
            }
        }
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

    public void StartSlideOut()
    {
        StartCoroutine(BossSlideOut());
    }

    private IEnumerator BossSlideOut()
    {
        while (Vector3.Distance(transform.position, EndPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                EndPosition,
                SlideSpeed * Time.unscaledDeltaTime
            );
            Debug.Log("move");
            yield return null; // Wait for next frame
        }

        // Snap exactly to target to avoid float error
        transform.position = EndPosition;

        // Now activate logs
        ActivateAllLogs();
    }

    private void DeactivateAllLogs()
    {
        Debug.Log("deactivating logs");
        foreach (GameObject log in logs)
        {
            if (log != null)
                log.SetActive(false);
        }
    }

    private void ActivateAllLogs()
    {
        foreach (GameObject log in logs)
            {
                if (log != null)
                    log.SetActive(true);
            }
    }

}
