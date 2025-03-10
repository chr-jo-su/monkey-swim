using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealthBar : HealthBar
{
    // Variables
    private bool gameOver = false;
    protected bool shake = false;
    protected Vector3 barPosition;
    [SerializeField] protected float lowHealth;

    private new void Start()
    {
        base.Start();
        barPosition = healthSlider.transform.localPosition;
        PlayerScore.instance.SetAtBossLevel(true);
    }

    /// <summary>
    /// Check if the boss is at low health.
    /// </summary>
    private new void Update()
    {
        base.Update();

        if (health / maxHealth < lowHealth)
        {
            shake = true;
        }
        else if (shake)
        {
            shake = false;
        }
    }

    /// <summary>
    /// Shake the health bar when the player's health is low.
    /// </summary>
    private void FixedUpdate()
    {
        if (barPosition != new Vector3())
        {
            if (shake)
            {
                healthSlider.transform.localPosition = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0);
                damageSlider.transform.localPosition = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0);
            }
            else
            {
                healthSlider.transform.localPosition = barPosition;
                damageSlider.transform.localPosition = barPosition;
            }
        }
    }

    /// <summary>
    /// Damages the boss for the given amount of health. Also prints out the player's score and shows the game over screen when the boss dies.
    /// </summary>
    /// <param name="damage">The amount of damage to give.</param>
    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (base.GetHealth() <= 0 && !gameOver)
        {
            StartCoroutine(LoadGameOverScreen());
            gameOver = true;
        }
    }
}