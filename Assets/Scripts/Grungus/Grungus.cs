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

    [SerializeField] private BossHealthBar bossHealth;

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
            timer = 0;
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
            animator.ResetTrigger("LeftAttack");
            direction = "left";
        }
        else
        {
            animator.ResetTrigger("RightAttack");
            animator.SetTrigger("LeftAttack");
            direction = "right";
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            bossHealth.TakeDamage(BananarangDamage.instance.GetDamage());
            //if (!soundeffects)
            //    soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();

            //int idx = Random.Range(0, painsounds.Length);
            //soundeffects.PlayOneShot(painsounds[idx]);

            //GetComponent<SpriteRenderer>().color = Color.red;
            //colorTimer = 0.1f;
        }
        else if (collision.gameObject.name == "Player")
        {
            PlayerHealthBar.instance.TakeDamage(25);
        }
    }
}
