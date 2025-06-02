using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grungus : MonoBehaviour
{
    // set phase to 0 to make the boss phase out
    public bool appear = true;
    private float progress = 0;
    private Color color;
    private Color visible = new Color(1f, 1f, 1f, 1f);
    private Color invisible = new Color(1f, 1f, 1f, 0f);

    void Start()
    {
        Mathf.Clamp(progress, 0, 1);
    }

    void Update()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;

        if (appear)
        {
            progress += Time.deltaTime;
        }
        else 
        {
            progress -= Time.deltaTime;
        }
        Phase();
    }

    void Phase() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(invisible, visible, progress);
    }
}
