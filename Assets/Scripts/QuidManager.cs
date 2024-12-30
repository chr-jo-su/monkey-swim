using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuidManager : BossFightManager
{
    public static BossFightManager instance;
    public BossSlide boss;

    private void Awake()
    {
        instance = this;
        // boss.SlideIn = false;
        // Debug.Log("Instance is running");
    }

    void Update()
    {
        // boss.SlideIn = false;
        base.Update();
        // TentacleManager.instance.enabled = true;
        // QuidManager.instance.enabled = false;
    }
}
