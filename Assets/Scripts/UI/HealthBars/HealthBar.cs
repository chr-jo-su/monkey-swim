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
    protected void Start()
    {
        health = maxHealth;

        healthSlider.value = health;
        damageSlider.value = health;

        healthSlider.maxValue = maxHealth;
        damageSlider.maxValue = maxHealth;
    }

    /// <summary>
    /// Updates the health bar to reflect the current health of the player.
    /// </summary>
    protected void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, lerpSpeed * 2);
        }

        if (healthSlider.value != damageSlider.value)
        {
            damageSlider.value = Mathf.Lerp(damageSlider.value, health, lerpSpeed);
        }
    }

    /// <summary>
    /// Changes the max health of the character by the given value. Can be either negative or positive. This value also changes the current health of the character to be in the range of [0, maxHealth].
    /// </summary>
    /// <param name="val">The value to change the max health by.</param>
    public void ChangeMaxHealth(int val)
    {
        maxHealth += val;

        health = Math.Min(health, maxHealth);
    }

    /// <summary>
    /// Damages the character for the given amount of health.
    /// </summary>
    /// <param name="val">The amount of damage to give.</param>
    public void TakeDamage(int val)
    {
        health = Math.Max(0, health - val);
    }

    /// <summary>
    /// Heals the character for the given amount of health until the character reaches max health.
    /// </summary>
    /// <param name="val">The amount of health to heal.</param>
    public void Heal(int val)
    {
        health = Math.Min(health + val, maxHealth);
    }

    /// <summary>
    /// Returns the current health of the character.
    /// </summary>
    /// <returns>Integer value of character's health.</returns>
    public float GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Returns the max health of the character.
    /// </summary>
    /// <returns>Integer value of the character's max health.</returns>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Loads the game over scene and unloads the current scene. Doesn't use the transition screen.
    /// </summary>
    /// <returns>An enumerator that's used when running as a coroutine.</returns>
    protected IEnumerator LoadGameOverScreen()
    {
        //Debug.Log("Final score: " + PlayerScore.instance.GetScore());
        //Debug.Log("Raw score: ");
        //foreach (int score in PlayerScore.instance.GetRawScore())
        //{
        //    Debug.Log("    " + score);
        //}

        string gameOverScene = "GameOver";
        SceneManager.LoadScene(gameOverScene, LoadSceneMode.Additive);

        // Set the new scene as the default and unload the old scene
        string oldSceneName = SceneManager.GetActiveScene().name;

        while (!SceneManager.GetSceneByName(gameOverScene).isLoaded) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameOverScene));

        // Set the scene that the game over scene will change to
        if (oldSceneName == "KrakenBoss")
        {
            SceneManager.GetActiveScene().GetRootGameObjects()[1].GetComponentInChildren<GameOverSceneChanger>().sceneName = "Level1";
        } else
        {
            SceneManager.GetActiveScene().GetRootGameObjects()[1].GetComponentInChildren<GameOverSceneChanger>().sceneName = "Level2";
        }

            Scene oldScene = SceneManager.GetSceneByName(oldSceneName);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
        while (!asyncUnload.isDone) yield return null;
    }
}
