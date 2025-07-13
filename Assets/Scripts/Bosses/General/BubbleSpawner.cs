using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float bubbleSpawnChance = 50.0f;
    public float spawnInterval = 0.5f;

    private float randomNumber;
    private Vector2 randomVector;

    public float spawnLocationY = -10;
    // THESE TWO X VALUES ARE A RANGE; THE BUBBLES WILL SPAWN SOMEWHERE INBETWEEN
    public float spawnLocationXMin = -10;
    public float spawnLocationXMax = 10;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateSpawner), 0, spawnInterval);
    }

    private void UpdateSpawner()
    {
        randomNumber = Random.Range(0.0f, 100.0f);

        if (randomNumber <= bubbleSpawnChance)
        {
            randomVector = new Vector2(Random.Range(spawnLocationXMin,
                spawnLocationXMax), spawnLocationY);

            Instantiate(bubblePrefab, randomVector, Quaternion.identity);
        }
    }
}
