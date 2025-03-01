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

    private float clearTimer = 1;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateSpawner", 0, spawnInterval);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
                    SpawnFish(i);
                }
            }
        }

        if (clearTimer <= 0)
        {
            foreach (GameObject fish in spawnedFishes)
            {
                if (fish.gameObject == null)
                {
                    spawnedFishes.Remove(fish);
                }
            }
            clearTimer = 1;
        }
        else
            clearTimer -= Time.deltaTime;
    }

    void SpawnFish(int index)
    {
        randomVector = new Vector2(
            Random.Range(transform.position.x - transform.localScale.x / 2,
            transform.position.x + transform.localScale.x / 2),
            Random.Range(transform.position.y - transform.localScale.y / 2,
            transform.position.y + transform.localScale.y / 2));

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(randomVector);
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1;

        while (isInView)
        {
            randomVector = new Vector2(
                Random.Range(transform.position.x - transform.localScale.x / 2,
                transform.position.x + transform.localScale.x / 2),
                Random.Range(transform.position.y - transform.localScale.y / 2,
                transform.position.y + transform.localScale.y / 2));

            viewportPoint = mainCamera.WorldToViewportPoint(randomVector);
            isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1;
        }

        GameObject newFish = Instantiate(possibleFishToSpawn[index],
            randomVector,
            Quaternion.identity);

        spawnedFishes.Add(newFish);
    }
}
