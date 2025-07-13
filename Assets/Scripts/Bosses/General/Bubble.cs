using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public PlayerMovement player;
    public int oxygenGain = 10;
    public float speed = 4.0f;
    public float deletePositionY = 20.0f;

    public AudioSource soundeffects;
    public AudioClip bubblePop;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        soundeffects = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
        if (soundeffects == null)
        {
            soundeffects = GameObject.Find("effectsSource").GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);

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

            soundeffects.PlayOneShot(bubblePop);
        }
    }
}
