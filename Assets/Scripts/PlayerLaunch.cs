using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{

    public float forceFactor; // how much to multiply the force acted upon the monkey
    public Rigidbody2D rb;
    private GameObject selectedObject;
    public GameObject playerObject;
    public GameObject mainCamera;
    private Vector2 velocity = Vector3.zero; // not sure why i need this exactly

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject && selectedObject.name == "playerlaunch")
        { 
            Vector2 forceDirection = new Vector2(-(mousePosition.x - rb.position.x), -(mousePosition.y - rb.position.y));

            rb.AddForce(forceDirection * forceFactor);

            Debug.Log("Released with a force of " + forceDirection.x * forceFactor + ", " + forceDirection.y * forceFactor);

            selectedObject = null;
        }
        else if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "sea")
        {
            // !!!: make sure there exists an instance of the player object cuz this clones it
            GameObject newPlayer = Instantiate(playerObject, (Vector2)gameObject.transform.position, Quaternion.identity);
            mainCamera.transform.SetParent(newPlayer.transform);
            mainCamera.transform.position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -10);
            newPlayer.GetComponent<Rigidbody2D>().AddForce(rb.velocity, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}
