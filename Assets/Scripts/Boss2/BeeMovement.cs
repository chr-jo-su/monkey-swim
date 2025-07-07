using UnityEngine;
using System.Collections;

public class BeeMovement : MonoBehaviour {
    public GameObject player;
    public float moveSpeed = 5;
    public float rotationSpeed = 5;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // rotate to look at the player
        Vector3 direction = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        direction.y = 0f;
        // move towards the player
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
