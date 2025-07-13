using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornMissileMovement : MonoBehaviour
{
    // Variables
    private GameObject targetObject;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float angularSpeed = 70f; // degrees per second
    [SerializeField] private int damage = 7;
    [SerializeField] private int bossDamage = 50;

    [SerializeField] private GameObject healthSystem;
    [SerializeField] private EnemyHealthBar fishHealth;
    private BossHealthBar bossHealthBar;

    private float colorTimer = 0;

    // Deviation variables
    private float deviationAmplitude = 1f;  // radians, adjust for more/less deviation
    private float deviationFrequency = 1f;  // oscillations per second

    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        bossHealthBar = GameObject.Find("BossHealthBar").GetComponentInChildren<BossHealthBar>();
    }

    void Update()
    {
        // Direction vector from missile to target
        Vector3 directionToTarget = (targetObject.transform.position - transform.position).normalized;

        // Calculate deviation angle using cosine
        float deviationAngle = Mathf.Cos(Time.time * deviationFrequency) * deviationAmplitude;

        // Apply deviation by rotating the direction vector
        Vector3 deviatedDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * deviationAngle, Vector3.forward) * directionToTarget;

        // Angle between missile's forward and deviated target direction
        float angle = Vector3.SignedAngle(transform.up, deviatedDirection, Vector3.forward);

        // Calculate angular velocity and rotate towards target
        float step = angularSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angle, -step, step);
        transform.Rotate(0, 0, rotationAmount);

        // Move forward
        transform.position += moveSpeed * Time.deltaTime * transform.up;

        if (colorTimer <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            colorTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == targetObject)
        {
            PlayerHealthBar.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.name == "Bananarang(Clone)")
        {
            fishHealth.TakeDamage(BananarangDamage.instance.GetDamage());

            if (fishHealth.GetHealth() <= 0.0f)
            {
                bossHealthBar.TakeDamage(bossDamage);
                Destroy(gameObject);
            }
            else if (fishHealth.GetHealth() < fishHealth.GetMaxHealth())
            {
                healthSystem.transform.localScale = new Vector3(0.2f, 0.1f, 1);
            }

            GetComponent<SpriteRenderer>().color = Color.red;
            colorTimer = 0.1f;
        }
    }
}
