using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : BossFightManager
{
    public static BeeManager instance;
    public BossSlide boss;

    private void Awake()
    {
        instance = this;
    }

    public void TurnOn()
    {
        running = true;
    }
    public bool checkBees() {
        return (bossObjects == null || bossObjects.Length == 0);
    }
}
