using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public float rockSpawnChance = 50.0f;
    public float spawnInterval = 0.5f;

    private float randomNumber;
    private Vector2 randomVector;

    public float spawnLocationYMin = 10;
    public float spawnLocationYMax = 15;
    public float spawnLocationXMin = -10;
    public float spawnLocationXMax = 10;

    private void Start()
    {
        InvokeRepeating("UpdateSpawner", 0, spawnInterval);
    }

    void UpdateSpawner()
    {
        randomNumber = Random.Range(0.0f, 100.0f);

        if (randomNumber <= rockSpawnChance)
        {
            randomVector = new Vector2(Random.Range(spawnLocationXMin,
                spawnLocationXMax), Random.Range(spawnLocationYMin,
                spawnLocationYMax));

            Instantiate(rockPrefab, randomVector, Quaternion.identity);
        }
    }
}
