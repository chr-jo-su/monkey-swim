using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananarangDamage : MonoBehaviour
{
    // Variables
    public static BananarangDamage instance;
    [SerializeField] private int damage = 15;

    private void Awake()
    {
        instance = this;
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