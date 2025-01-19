using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBubble : MonoBehaviour
{
    public PlayerMovementAndOxygen player;
    public float oxygenGain = 10.0f;
    public float speed = 4.0f;
    public float deletePositionY = 20.0f;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovementAndOxygen>();
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y >= deletePositionY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.oxygen += oxygenGain;
            Destroy(gameObject);
        }
    }
}
