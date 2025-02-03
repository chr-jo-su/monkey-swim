using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealthBar : HealthBar
{
    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}