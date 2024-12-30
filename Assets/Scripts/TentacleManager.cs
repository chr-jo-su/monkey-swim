using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleManager : BossFightManager
{
    public static TentacleManager instance;
    public BossSlide boss;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // boss.SlideIn = true;
        base.Update();
        // QuidManager.instance.enabled = true;
        // TentacleManager.instance.enabled = false;
    }
}
