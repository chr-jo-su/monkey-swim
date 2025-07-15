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
    public Animator animator;
    private bool rightAttack;
    private bool leftAttack;
    

    private void Start()
    {
        InvokeRepeating("UpdateSpawner", 0, spawnInterval);
    }

    void Update() {
        rightAttack = animator.GetBool("RightAttack");
        leftAttack = animator.GetBool("LeftAttack");

        float y = 15;
        if (leftAttack == true) {
            for (int x = 0; x <= 14; x += 2) {
                Instantiate(rockPrefab, new Vector2(x, y), Quaternion.identity);
                y -= 0.5f;
            }
        } else if (rightAttack == true) {
            for (int x = -14; x <= 0; x += 2) {
                Instantiate(rockPrefab, new Vector2(x, y), Quaternion.identity);
                y += 0.5f;
            }
        }
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
