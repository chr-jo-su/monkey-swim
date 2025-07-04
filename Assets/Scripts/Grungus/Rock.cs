using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public PlayerMovement player;
    public float speed = 4.0f;
    public float deletePositionY = -20.0f;

    public AudioSource soundeffects;
    public AudioClip bubblePop;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        // transform.Rotate(Vector2.right);

        if (transform.position.y <= deletePositionY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(gameObject);

            soundeffects.PlayOneShot(bubblePop);
        }
    }
}
