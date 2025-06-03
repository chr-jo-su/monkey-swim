using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGoatManager : MonoBehaviour
{
    // Variables
    public static SeaGoatManager instance;

    [SerializeField] private StageType stage = StageType.Start;

    public void Awake()
    {
        instance = this;
    }

    public void ChangeStage(StageType type)
    {
        stage = type;
    }
}

public enum StageType
{
    None,
    Start,
    Idle,
    Dash,
    HornMissile
}
