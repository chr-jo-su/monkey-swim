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
}
