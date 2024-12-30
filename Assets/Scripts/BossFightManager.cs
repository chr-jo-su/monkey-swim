using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public GameObject[] bossObjects;
    //public int[][] spawnPosition;
    public int[] spawnPositionX;
    public int[] spawnPositionY;
    public float[] spawnTimes;

    private float time = 0;
    private int counter = 0;
    private float timerMax = 0;
    private bool[] alreadySpawned;

    private void Start()
    {
        foreach (float s in spawnTimes) {
            if (s > timerMax)
            {
                timerMax = s;
            }
        }

        alreadySpawned = new bool[bossObjects.Length];
        // Debug.Log(alreadySpawned.Length);
    }

    protected void Update()
    {

        foreach (double s in spawnTimes) {
            if (time >= s && alreadySpawned[counter] == false) {
                Vector2 spawnPos = new Vector2(spawnPositionX[counter], spawnPositionY[counter]);
                Instantiate(bossObjects[counter], spawnPos, Quaternion.identity);
                alreadySpawned[counter] = true;
            }
            // Debug.Log(counter);
            counter++;
        }
        counter = 0;

        if (time >= timerMax)
        {
            time = 0;
            alreadySpawned = new bool[bossObjects.Length];
        } else
        {
            time += Time.deltaTime;
        }
    }
}
