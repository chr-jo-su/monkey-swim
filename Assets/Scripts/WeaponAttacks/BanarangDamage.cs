using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BanarangDamage : HealthBar
{
    // Variables
    public static BanarangDamage instance;
    private int damage;

    private void Awake()
    {
        instance = this;
    }
    private new void Start()
    {
        damage = 15;
    }


    public int GetDamage()
    {
        Debug.Log("damage");
        return damage;
    }
    public void ChangeDamage(int d) {
        damage += d;
    }
}