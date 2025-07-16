using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Variables
    public int oxygenGain = 10;
    public float speed = 4.0f;
    public float deletePositionY = 20.0f;

    public AudioSource soundeffects;
    public AudioClip bubblePop;

    // Start is called before the first frame update
    private void Start()
    {
        soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);

        if (transform.position.y >= deletePositionY)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// It checks whether the player is in the bubble and gives the player some oxygen.
    /// </summary>
    /// <param name="collision"></param>
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
