using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    // Variables
    [SerializeField] protected GameObject[] bossObjects;
    [SerializeField] private int[] spawnPositionX;
    [SerializeField] private int[] spawnPositionY;
    [SerializeField] private float[] spawnTimes;

    private float time = 0;
    private int counter = 0;
    private float timerMax = 0;
    private bool[] alreadySpawned;
    protected bool running = true;

    // Start is called before the first frame update
    private void Start()
    {
        foreach (float s in spawnTimes)
        {
            if (s > timerMax)
            {
                timerMax = s;
            }
        }

        alreadySpawned = new bool[bossObjects.Length];
    }

    // Update is called once per frame
    protected void Update()
    {
        if (running)
        {
            foreach (double s in spawnTimes)
            {
                if (time >= s && alreadySpawned[counter] == false)
                {
                    Vector2 spawnPos = new Vector2(spawnPositionX[counter], spawnPositionY[counter]);
                    Instantiate(bossObjects[counter], spawnPos, Quaternion.identity);
                    alreadySpawned[counter] = true;
                }

                counter++;
            }

            counter = 0;

            if (time >= timerMax)
            {
                time = 0;
                alreadySpawned = new bool[bossObjects.Length];
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Turns off the current boss.
    /// </summary>
    public void TurnOff()
    {
        running = false;
    }
}
