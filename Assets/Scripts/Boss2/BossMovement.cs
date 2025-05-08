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
    // public static BossMovement instance;

    public float health = 500.0f;
    public BossHealthBar bossHealth;

    public PlayerMovement player;
    // Start is called before the first frame update
    public PlayerMovement player;
    public int oxygenLoss = -15;
    public float speed = 0.0f;
    public float deletePositionY = 20.0f;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y >= deletePositionY)
        {
            // Destroy(gameObject);
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
}
