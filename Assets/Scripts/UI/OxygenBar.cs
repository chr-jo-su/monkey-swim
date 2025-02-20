using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    // Variables
    public static OxygenBar instance;

    [SerializeField] protected Slider oxygenSlider;

    private bool canBreath = true;
    private float oxygen;
    [SerializeField] private float maxOxygen = 100.0f;
    [SerializeField] protected int oxygenGainRate = 2;
    [SerializeField] protected int oxygenDepletionRate = 1;
    [SerializeField] protected float lowOxygen;
    [SerializeField] private int oxygenDamage = 3;
    [SerializeField] private int drownTimer = 100;
    private int currentDrownTimer = 0;

    private bool isMaxOxygen = true;
    private float oxygenBarRatio;
    protected Vector3 barPosition;
    private bool shake = false;
    [SerializeField] private int shakeAmount = 3;

    /// <summary>
    /// Creates a singleton instance of the OxygenBar.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Sets the oxygen level to the max oxygen level when the game starts.
    /// </summary>
    private void Start()
    {
        oxygen = maxOxygen;
        oxygenSlider.value = oxygen;

        oxygenBarRatio = oxygenSlider.transform.localScale.x / maxOxygen;
        barPosition = oxygenSlider.transform.localPosition;
    }

    /// <summary>
    /// Decreases the oxygen level of the player if the player is not able to breathe. Otherwise, increases the oxygen level of the player.
    /// </summary>
    void Update()
    {
        if (oxygen > 0.0f && !canBreath)
        {
            oxygen -= oxygenDepletionRate * Time.deltaTime;
            oxygenSlider.value = oxygen;
        }
        else if (oxygen < maxOxygen && canBreath)
        {
            oxygen += oxygenGainRate * Time.deltaTime;
            oxygenSlider.value = oxygen;
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

        if (oxygen / maxOxygen < lowOxygen)
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
                oxygenSlider.transform.localPosition = new Vector3(barPosition.x + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.y + UnityEngine.Random.Range(-shakeAmount, shakeAmount), barPosition.z);
            }
            else
            {
                oxygenSlider.transform.localPosition = barPosition;
            }
        }
    }

    /// <summary>
    /// Changes the max oxygen of the player by the given value. Can be either negative or positive. This value also changes the current oxygen of the player to be in the range of [0, maxOxygen].
    /// </summary>
    /// <param name="val">The value to change the maxOxygen variable by.</param>
    public void ChangeMaxOxygen(int val)
    {
        maxOxygen += val;

        if (isMaxOxygen)
        {
            oxygen = maxOxygen;
        }
        else
        {
            oxygen = Math.Min(oxygen, maxOxygen);
            oxygen += val;
        }

        ChangeOxygenBarSize();
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
    /// Change the size of the oxygen bar.
    /// </summary>
    public void ChangeOxygenBarSize()
    {
        oxygenSlider.transform.localScale = new Vector3(oxygenBarRatio * maxOxygen, oxygenSlider.transform.localScale.y, oxygenSlider.transform.localScale.z);
        oxygenSlider.maxValue = maxOxygen;
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
