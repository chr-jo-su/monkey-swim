using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    // Variables
    public static PlayerHealthBar instance;

    private bool isMaxHealth = true;
    private bool gameOver = false;
    protected bool shake = false;

    private float healthBarRatio;
    protected Vector3 barPosition;
    [SerializeField] protected float lowHealth;
    [SerializeField] private int shakeAmount = 3;

    /// <summary>
    /// Creates a singleton instance of the PlayerHealthBar.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Sets the health to the max health when the game starts.
    /// </summary>
    private new void Start()
    {
        base.Start();

        healthBarRatio = healthSlider.transform.localScale.x / maxHealth;
        barPosition = healthSlider.transform.localPosition;
    }

    /// <summary>
    /// Check if the player is at low health
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
                healthSlider.transform.localPosition = new Vector3(barPosition.x + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.y + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.z);
                damageSlider.transform.localPosition = new Vector3(barPosition.x + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.y + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.z);
            }
            else
            {
                healthSlider.transform.localPosition = barPosition;
                damageSlider.transform.localPosition = barPosition;
            }
        }
    }

    /// <summary>
    /// Damages the player for the given amount of health. This function also checks if the player has died (gotten to a health of 0) and loads the game over screen if they have.
    /// </summary>
    /// <param name="val">The amount of damage the player takes.</param>
    public new void TakeDamage(int val)
    {
        base.TakeDamage(val);
        isMaxHealth = false;

        if (base.GetHealth() <= 0 && !gameOver)
        {
            StartCoroutine(LoadGameOverScreen());
            gameOver = true;
        }
    }

    /// <summary>
    /// Changes the max health of the player by the given value. Can be either negative or positive. This value also changes the current health of the player to be in the range of [0, maxHealth].
    /// </summary>
    /// <param name="val">The value to change the maxHealth variable by.</param>
    public new void ChangeMaxHealth(int val)
    {
        maxHealth += val;

        if (isMaxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = Math.Min(health, maxHealth);
        }

        ChangeHealthBarSize();
    }

    /// <summary>
    /// Heals the player for the given amount of health. This function also checks if the player has reached max health and sets the isMaxHealth variable accordingly.
    /// </summary>
    /// <param name="val">The amount of health to heal.</param>
    public new void Heal(int val)
    {
        base.Heal(val);

        if (health == maxHealth)
        {
            isMaxHealth = true;
        }
    }

    /// <summary>
    /// Change the size of the health bar.
    /// </summary>
    public void ChangeHealthBarSize()
    {
        healthSlider.transform.localScale = new Vector3(healthBarRatio * maxHealth, healthSlider.transform.localScale.y, healthSlider.transform.localScale.z);
        damageSlider.transform.localScale = new Vector3(healthBarRatio * maxHealth, damageSlider.transform.localScale.y, damageSlider.transform.localScale.z);

        healthSlider.maxValue = maxHealth;
        damageSlider.maxValue = maxHealth;
    }

    /// <summary>
    /// Loads the game over scene and unloads the current scene.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    private IEnumerator LoadGameOverScreen()
    {
        string gameOverScene = "GameOver";
        SceneManager.LoadScene(gameOverScene, LoadSceneMode.Additive);

        // Set the new scene as the default and unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(gameOverScene).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameOverScene));

        Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}