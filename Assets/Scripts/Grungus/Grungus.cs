using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grungus : MonoBehaviour
{
    // set phase to 0 to make the boss phase out
    public bool appear = true;
    private float progress = 0;
    private Color visible = new Color(1f, 1f, 1f, 1f);
    private Color invisible = new Color(1f, 1f, 1f, 0f);
    private float timer = 0;
    private String direction = "left";
    public Animator animator;

    void Start()
    {
        Mathf.Clamp(progress, 0, 1);
    }

    void Update()
    {
        if (appear)
        {
            progress += Time.deltaTime;
        }
        else
        {
            progress -= Time.deltaTime;
        }
        Phase();

        timer += Time.deltaTime;

        if (timer >= 3)
        {
            Attack();
            timer = -3;
        }
    }

    void Phase()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(invisible, visible, progress);
    }

    void Attack()
    {
        if (direction == "right")
        {
            animator.SetTrigger("RightAttack");
            direction = "left";
        }
        else
        {
            animator.SetTrigger("LeftAttack");
            direction = "right";
        }
    }
}
