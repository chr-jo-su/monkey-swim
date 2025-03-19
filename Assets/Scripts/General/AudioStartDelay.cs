using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStartDelay : MonoBehaviour
{
    public AudioSource audioSource;
    public float delay = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (delay >= 0.0f)
            delay -= Time.deltaTime;
        else
        {
            audioSource.Play();
            Destroy(this);
        }
    }
}
