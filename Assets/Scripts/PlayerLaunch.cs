using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{

    public float forceFactor; // how much to multiply the force acted upon the monkey
    public Rigidbody2D rb;
    private GameObject selectedObject;

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
}
