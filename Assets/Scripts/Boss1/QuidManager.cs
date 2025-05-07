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
    }
}
