using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerLaunch : MonoBehaviour
{

    public float forceMultiplier = 1F; // how much to multiply the force acted upon the monkey
    public Rigidbody2D rigidBody;
    private GameObject selectedObject;
    public GameObject playerObject;
    public Camera mainCamera;
    public float maxForceX = 200F;
    public float minForceX = -200F;
    public float maxForceY = 200F;
    public float minForceY = -200F;
    public GameObject seaLineObject; // should be a thin object with a boxCollider2D component and trigger enabled
                                     // that signifies the sea line
                                     
    public AudioSource underWaterMusic;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject && selectedObject.name == name)
        { 
            // apologies for this ungodly long line. i will surely shorten it sometime. surely.
            Vector2 forceDirection = new Vector2(Mathf.Clamp(-(mousePosition.x - rigidBody.position.x) * forceMultiplier, minForceX, maxForceX), Mathf.Clamp(-(mousePosition.y - rigidBody.position.y) * forceMultiplier, minForceY, maxForceY));
            
            rigidBody.AddForce(forceDirection);

            Debug.Log("Released with a force of " + forceDirection.x + ", " + forceDirection.y);
            
            mainCamera.GetComponent<SmoothFollowCamera>().enabled = true; 
            mainCamera.GetComponent<SmoothFollowCamera>().playerObject = gameObject;

            selectedObject = null;
        }
        else if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }
    }

    // when the playerLaunch enters the sea, clone existing instance of Player and delete self
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == seaLineObject.name)
        {
            // !!!: make sure there exists an instance of the player object cuz this clones it
            underWaterMusic.GetComponent<AudioStartDelay>().enabled = true;
            //GameObject newPlayer = Instantiate(playerObject, (Vector2)gameObject.transform.position, Quaternion.identity);
            playerObject.transform.position = (Vector2)gameObject.transform.position;
            //newPlayer.GetComponent<Rigidbody2D>().AddForce(rigidBody.velocity, ForceMode2D.Impulse);
            playerObject.GetComponent<Rigidbody2D>().AddForce(rigidBody.velocity, ForceMode2D.Impulse);
            mainCamera.GetComponent<SmoothFollowCamera>().playerObject = playerObject;
            Destroy(gameObject);
        }
    }
}
