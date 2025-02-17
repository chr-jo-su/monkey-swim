using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Variables
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Slider damageSlider;

    [SerializeField] protected float maxHealth;
    protected float health;

    private readonly float lerpSpeed = 0.1f;

    /// <summary>
    /// Sets the health to the max health when the game starts.
    /// </summary>
    void Start()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Updates the health bar to reflect the current health of the player.
    /// </summary>
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != damageSlider.value)
        {
            damageSlider.value = Mathf.Lerp(damageSlider.value, health, lerpSpeed);
        }
    }

    /// <summary>
    /// Changes the max health of the player by the given value. Can be either negative or positive. This value also changes the current health of the player to be in the range of [0, maxHealth].
    /// </summary>
    /// <param name="val">The value to change the max health by.</param>
    public void ChangeMaxHealth(int val)
    {
        maxHealth += val;

        health = Math.Min(health, maxHealth);
    }

    /// <summary>
    /// Damages the player for the given amount of health.
    /// </summary>
    /// <param name="val">The amount of damage to give.</param>
    public void TakeDamage(int val)
    {
        health = Math.Max(0, health - val);
    }

    /// <summary>
    /// Heals the player for the given amount of health until the player reaches max health.
    /// </summary>
    /// <param name="val">The amount of health to heal.</param>
    public void Heal(int val)
    {
        health = Math.Max(health + val, maxHealth);
    }

    /// <summary>
    /// Returns the current health of the player.
    /// </summary>
    /// <returns>Integer value of player's health.</returns>
    public float GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Returns the max health of the player.
    /// </summary>
    /// <returns>Integer value of the player's max health.</returns>
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
