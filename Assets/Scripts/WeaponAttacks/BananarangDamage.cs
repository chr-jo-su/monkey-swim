using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BananarangDamage : MonoBehaviour
{
    // Variables
    public static BananarangDamage instance;
    private int damage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        damage = 15;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void ChangeDamage(int d) {
        damage += d;
    }

    public void ResetInstance()
    {
        instance = this;
    }

}