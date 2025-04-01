using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    // Variables
    public static PlayerScore instance;

    private float timeAlive;
    private int fishKilled;
    private bool atBossLevel = false;
    private float depth = 0;

    /// <summary>
    /// Creates a singleton instance of the PlayerScore.
    /// </summary>
    private void Awake()
    {
        instance = this;

        timeAlive = 0;
        fishKilled = 0;
    }

    /// <summary>
    /// Adds a point to the player's score every update. (roughly translates to time alive in seconds)
    /// </summary>
    private void FixedUpdate()
    {
        timeAlive += 0.015f;

        if (!atBossLevel)
        {
            depth = Mathf.Min(transform.position.y, depth);
        }
    }

    /// <summary>
    /// Returns the player's true score.
    /// </summary>
    /// <returns>An integer representing the player's score.</returns>
    public double GetScore()
    {
        //float finalScore = (atBossLevel ? 1000 : (Mathf.Abs(depth) * 10)) + (fishKilled * 100) + timeAlive;
        float finalScore = timeAlive;

        return (int)finalScore;
    }

    /// <summary>
    /// Returns the player's raw scores before computation.
    /// </summary>
    /// <returns>An array of integers representing the player's raw scores.</returns>
    public List<int> GetRawScore()
    {
        return new() { (int)Mathf.Round(timeAlive), fishKilled, atBossLevel ? 1 : 0, (int)Mathf.Round(depth) };
    }

    /// <summary>
    /// Resets the player's score to 0.
    /// </summary>
    public void ResetScore()
    {
        timeAlive = 0f;
        fishKilled = 0;
    }

    /// <summary>
    /// Increment fishKilled by 1.
    /// </summary>
    public void IncrementFishKilled()
    {
        fishKilled++;
    }

    /// <summary>
    /// Sets the atBossLevel boolean to true or false.
    /// </summary>
    /// <param name="value">The boolean value to save to atBossLevel.</param>
    public void SetAtBossLevel(bool value)
    {
        atBossLevel = value;
    }
}
