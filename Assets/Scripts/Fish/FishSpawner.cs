using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Tooltip("list of possible fish that spawn. make sure index matches that" +
        "of its spawn chance.")]
    public GameObject[] possibleFishToSpawn;

    [Tooltip("fish spawn chance from 0 to 100. make sure index matches that " +
        "of its corresponding fish.")]
    public float[] fishSpawnChance;

    [Tooltip("spawn interval in seconds.")]
    public float spawnInterval = 1.0f;

    public int spawnCap = 3;

    private float randomNumber = -1.0f;

    private Vector2 randomVector;

    private List<GameObject> spawnedFishes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateSpawner", 0, spawnInterval);
    }

    void UpdateSpawner()
    {
        randomNumber = Random.Range(0.0f, 100.0f);

        if (spawnedFishes.Count < spawnCap)
        {
            for (int i = 0; i < possibleFishToSpawn.Length; i++)
            {
                if (randomNumber <= fishSpawnChance[i])
                {
                    Debug.Log("fish!!!");
                    SpawnFish(i);
                }
            }
        }

        foreach (GameObject fish in spawnedFishes)
        {
            //try
            //{
            //    if (!fish.activeSelf)
            //    {
            //        spawnedFishes.Remove(fish);
            //    }
            //}
            //catch (NullReferenceException)
            //{

            //}

            if (fish.gameObject == null)
            {
                spawnedFishes.Remove(fish);
            }
        }
    }

    void SpawnFish(int index)
    {
        randomVector = new Vector2(
            Random.Range(transform.position.x - transform.localScale.x / 2,
            transform.position.x + transform.localScale.x / 2),
            Random.Range(transform.position.y - transform.localScale.y / 2,
            transform.position.y + transform.localScale.y / 2));

        GameObject newFish = Instantiate(possibleFishToSpawn[index],
            randomVector,
            Quaternion.identity);

        spawnedFishes.Add(newFish);
    }
}
