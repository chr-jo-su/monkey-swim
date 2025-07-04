using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public PlayerMovement player;
    public float speed = 4.0f;
    private float Angle;
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
        Angle += 0.0008f;
        transform.rotation = quaternion.Euler(0, 0, Angle * Mathf.Rad2Deg);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

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
