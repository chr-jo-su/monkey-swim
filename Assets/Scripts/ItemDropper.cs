using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [Tooltip("list of items that can possibly be dropped")]
    public Item[] dropItems;
    
    [Tooltip("drop chance for each item (ensure same index number as corresponding" +
             " item in dropItems) with max chance value being 100.0f")]
    public float[] dropChance;

    public float minForce = 0.0f;
    public float maxForce = 4;

    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        
        float randX, randY, randForce;
        
        foreach (var i in dropItems)
        {
            if (Random.Range(0.0f, 100.0f) <= dropChance[counter])
            {
                GameObject droppedItem = new GameObject();
                droppedItem.transform.position = transform.parent.position;

                droppedItem.name = i.itemName;

                SpriteRenderer spriteRender = droppedItem.AddComponent<SpriteRenderer>();
                spriteRender.sprite = i.itemSprite;

                Rigidbody2D rb = droppedItem.AddComponent<Rigidbody2D>();
                rb.drag = 1;
                rb.gravityScale = 0;

                randX = Random.Range(-1, 2);
                randY = Random.Range(-1, 2);
                randForce = Random.Range(minForce, maxForce);
                
                rb.AddForce(new Vector2(randX, randY) * randForce, ForceMode2D.Impulse);
                Debug.Log("Item released with with force of " + randX * randForce + "," + randY * randForce);
            }

            counter++;
        }
    }
}
