using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    // Variables
    public static OxygenBar instance;

    [SerializeField] protected GameObject oxygenSlider;

    private bool canBreath = true;
    private float oxygen = 100.0f;
    [SerializeField] private float maxOxygen = 100.0f;
    [SerializeField] protected float oxygenChangeRate = 1.0f;
    [SerializeField] private int oxygenDamage = 3;
    [SerializeField] private int drownTimer = 100;
    private int currentDrownTimer = 0;

    /// <summary>
    /// Creates a singleton instance of the OxygenBar.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Decreases the oxygen level of the player if the player is not able to breathe. Otherwise, increases the oxygen level of the player.
    /// </summary>
    void Update()
    {
        if (oxygen > 0.0f && !canBreath)
        {
            oxygen -= oxygenChangeRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }
        else if (oxygen < maxOxygen && canBreath)
        {
            oxygen += 10 * oxygenChangeRate * Time.deltaTime;
            oxygenSlider.GetComponent<Slider>().value = oxygen * 0.01f;
        }

        if (oxygen <= 0.0f)
        {
            if (currentDrownTimer == drownTimer)
            {
                PlayerHealthBar.instance.TakeDamage(oxygenDamage);
                currentDrownTimer = 0;
            }
            currentDrownTimer++;
        }
    }

    /// <summary>
    /// Changes the oxygen level of the player by the given value.
    /// </summary>
    /// <param name="val">The value to change the oxygen level by. Can be negative.</param>
    public void ChangeOxygen(int val)
    {
        maxOxygen += val;

        if (val < 0)
        {
            oxygen = Math.Min(oxygen, maxOxygen);
        }
    }

    /// <summary>
    /// Sets the ability to breathe.
    /// </summary>
    /// <param name="val">The boolean value of whether the player loses oxygen or not.</param>
    public void SetBreathe(bool val)
    {
        canBreath = val;
    }
}
