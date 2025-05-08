using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeird : MonoBehaviour
{
    public PlayerMovement player;
    public int oxygenGain = -15;
    public float speed = 0.0f;
    public float deletePositionY = 20.0f;

    public AudioSource soundeffects;
    public AudioClip bubblePop;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
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
            Debug.Log("aOOga");
            OxygenBar.instance.ChangeOxygen(oxygenGain);
            PlayerMovement.instance.stopMovement();
            yield return new WaitForSeconds(3);
            PlayerMovement.instance.startMovement();
            // Destroy(gameObject);

            soundeffects.PlayOneShot(bubblePop);
        }
    }
}
