using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public float thrust;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            Rigidbody2D Fish = other.GetComponent<Rigidbody2D>();
            if (Fish != null)
            {
                Fish.isKinematic = false;
                Vector2 difference = Fish.transform.position - transform.position;
                difference = difference.normalized*thrust;
                Fish.AddForce(difference, ForceMode2D.Impulse);
                Fish.isKinematic = true;
            }
        }
    }
}
