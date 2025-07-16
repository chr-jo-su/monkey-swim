using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleManager : BossFightManager
{
    public static TentacleManager instance;
    public BossSlide boss;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }
}
