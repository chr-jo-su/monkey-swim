using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBubble : MonoBehaviour
{
    public PlayerMovement player;
    public int oxygenGain = 10;
    public float speed = 4.0f;
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OxygenBar.instance.ChangeOxygen(oxygenGain);
            Destroy(gameObject);
        }
    }
}
