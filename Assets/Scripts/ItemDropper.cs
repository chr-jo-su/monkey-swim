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
                droppedItem.tag = "Item";
                droppedItem.layer = 3;
                
                // I dont really know a better way to go about this :/
                DroppedItem itemProperties = droppedItem.AddComponent<DroppedItem>();
                itemProperties.item = i;

                SpriteRenderer spriteRender = droppedItem.AddComponent<SpriteRenderer>();
                spriteRender.sprite = i.itemSprite;
                
                // REMOVE THIS LATER WHEN ITEMS HAVE A SPRITE SIZE PROPERTY!!!!!!
                spriteRender.drawMode = SpriteDrawMode.Sliced;
                spriteRender.size = new Vector2(spriteRender.size.x * 0.10f, spriteRender.size.y * 0.10f);
                // -------------------------------------

                Rigidbody2D rb = droppedItem.AddComponent<Rigidbody2D>();
                rb.drag = 1;
                rb.gravityScale = 0;
                
                BoxCollider2D collider = droppedItem.AddComponent<BoxCollider2D>();
                collider.size = spriteRender.size;
                collider.isTrigger = true;

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
