using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrepAreaSlotHolder : MinigameSlotHolder, IDropHandler
{
    // Variables
    public static PrepAreaSlotHolder instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddIngredient(CookingIngredient ingredient, float scale)
    {
        GameObject newGameObjectItem = Instantiate(CookingIngredientPrefab, gameObject.transform);
        CookingItem itemInstance = newGameObjectItem.GetComponent<CookingItem>();

        itemInstance.InitialiseItem(ingredient);

        itemInstance.isOnPrepArea = true;
        itemInstance.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void RemoveIngredients()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// This adds the object to the slot if it is empty.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            CookingItem draggableItem = eventData.pointerDrag.GetComponent<CookingItem>();
            draggableItem.parentAfterDrag = transform;
            draggableItem.isOnPrepArea = true;
        }
    }
}
