using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CookingItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerClickHandler
{
    // Variables
    public Image image;
    [HideInInspector] public CookingIngredient storedIngredient;

    private float clickCount = 0;
    private float timeCount = 0f;

    private bool clickStarted = false;
    private bool timerStarted = false;

    private bool converted = false;

    [SerializeField] private Slider conversionBar;

    [SerializeField] private GameObject tooltipPrefab;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool draggable = true;

    private bool allowTooltip = true;

    public bool isOnPrepArea = false;

    private void FixedUpdate()
    {
        if (timerStarted && timeCount < storedIngredient.timeToConvert)
        {
            timeCount += Time.deltaTime;
        }
        else if (!converted && (timerStarted && timeCount >= storedIngredient.timeToConvert) || (clickStarted && clickCount >= storedIngredient.clicksToConvert))
        {
            converted = true;
            CookingAreaManager.instance.ConvertCurrentItem();
        }

        if (!isOnPrepArea)
        {
            timerStarted = false;
            timeCount = 0f;

            clickStarted = false;
            clickCount = 0;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else if (isOnPrepArea)
        {
            if (storedIngredient.ingredientType != IngredientType.None)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }

            if (storedIngredient.ingredientType == IngredientType.Clickable)
            {
                conversionBar.value = (storedIngredient.clicksToConvert - clickCount) / storedIngredient.clicksToConvert;
            }
            else if (storedIngredient.ingredientType == IngredientType.Timed)
            {
                conversionBar.value = (storedIngredient.timeToConvert - timeCount) / storedIngredient.timeToConvert;
            }
        }
    }

    /// <summary>
    /// Checks if the item was clicked on.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isOnPrepArea)
        {
            allowTooltip = true;
            ShowTooltip();
        }
        else if (storedIngredient.ingredientType == IngredientType.Clickable)
        {
            clickStarted = true;
            clickCount++;
            CookingMinigameManager.instance.PlaySFX(storedIngredient.GetRandomInProgressSFXClip());
        }
        else if (storedIngredient.ingredientType == IngredientType.Timed && !timerStarted)
        {
            timerStarted = true;
            CookingMinigameManager.instance.PlaySFX(storedIngredient.GetRandomInProgressSFXClip());
        }
    }

    /// <summary>
    /// Don't allow the tooltip to show if the mouse exits the object.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        allowTooltip = false;
    }

    /// <summary>
    /// Initialise the slot with the given image and quantity
    /// </summary>
    /// <param name="newIngredient">The Item scriptable object to be added.</param>
    public void InitialiseItem(CookingIngredient newIngredient, bool isDraggable = true)
    {
        storedIngredient = newIngredient;
        image.sprite = newIngredient.itemSprite;
        draggable = isDraggable;
    }

    /// <summary>
    /// Change the parent to the root object and set it to not be hit by a raycast.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }

    /// <summary>
    /// Move the item to the mouse position.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// Change the parent of the item to be the new slot and allow it to be hit by a raycast.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            CookingMinigameManager.instance.PlayClickSFX();
        }
    }

    /// <summary>
    /// Shows a menu for the inventory item when hovered over. The ItemTooltip script handles destroying the object.
    /// </summary>
    public void ShowTooltip()
    {
        if (allowTooltip)
        {
            // Instantiate the tooltip
            GameObject newGameObjectItem = Instantiate(tooltipPrefab, transform.root);

            // Get the height of the tooltip
            RectTransform rectTransform = newGameObjectItem.GetComponent<RectTransform>();
            float height = rectTransform.rect.height;
            float width = rectTransform.rect.width;

            // Set the position of the tooltip
            newGameObjectItem.transform.position = Input.mousePosition;

            // Check if the tooltip is off the screen and make it always show fully
            if (Input.mousePosition.y - height < 0)
            {
                newGameObjectItem.transform.position = new Vector3(newGameObjectItem.transform.position.x, height, newGameObjectItem.transform.position.z);
            }
            if (Input.mousePosition.x + width > Screen.width)
            {
                newGameObjectItem.transform.position = new Vector3(Screen.width - width, newGameObjectItem.transform.position.y, newGameObjectItem.transform.position.z);
            }

            // Set the text info for the item
            ItemTooltip tooltip = newGameObjectItem.GetComponent<ItemTooltip>();
            tooltip.SetItemInfoText(storedIngredient.ingredientName);
        }
    }
}
