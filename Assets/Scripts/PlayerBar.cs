using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : HealthBar
{
    public static HealthBar instance;

    private void Awake()
    {
        instance = this;
    }
}