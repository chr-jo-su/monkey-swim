using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthBar : HealthBar
{
    // Variables
    public static PlayerHealthBar instance;

    private bool isMaxHealth = true;
    private bool gameOver = false;
    protected bool shake = false;

    private float healthBarRatio;
    protected Vector3 barPosition;
    [SerializeField] protected float lowHealth;
    [SerializeField] private int shakeAmount = 3;

    /// <summary>
    /// Creates a singleton instance of the PlayerHealthBar.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Sets the health to the max health when the game starts.
    /// </summary>
    private new void Start()
    {
        base.Start();

        healthBarRatio = healthSlider.transform.localScale.x / maxHealth;
        barPosition = healthSlider.transform.localPosition;
    }

    /// <summary>
    /// Check if the player is at low health
    /// </summary>
    private new void Update()
    {
        base.Update();

        if (health / maxHealth < lowHealth)
        {
            shake = true;
        }
        else if (shake)
        {
            shake = false;
        }
    }

    /// <summary>
    /// Shake the health bar when the player's health is low.
    /// </summary>
    private void FixedUpdate()
    {
        if (barPosition != new Vector3())
        {
            if (shake)
            {
                healthSlider.transform.localPosition = new Vector3(barPosition.x + Random.Range(-shakeAmount, shakeAmount), barPosition.y + Random.Range(-shakeAmount, shakeAmount), barPosition.z);
                damageSlider.transform.localPosition = new Vector3(barPosition.x + Random.Range(-shakeAmount, shakeAmount), barPosition.y + Random.Range(-shakeAmount, shakeAmount), barPosition.z);
            }
            else
            {
                healthSlider.transform.localPosition = barPosition;
                damageSlider.transform.localPosition = barPosition;
            }
        }
    }

    /// <summary>
    /// Damages the player for the given amount of health.
    /// This function also checks if the player has died (gotten to a health of 0), and resets their score and loads the game over screen if they have.
    /// </summary>
    /// <param name="val">The amount of damage the player takes.</param>
    public new void TakeDamage(int val)
    {
        base.TakeDamage(val);
        isMaxHealth = false;

        if (base.GetHealth() <= 0 && !gameOver)
        {
            SceneTransitionManager();
            gameOver = true;
        }
    }

    /// <summary>
    /// Changes the max health of the player by the given value. Can be either negative or positive.
    /// This value also changes the current health of the player to be in the range of [0, maxHealth].
    /// </summary>
    /// <param name="val">The value to change the maxHealth variable by.</param>
    public new void ChangeMaxHealth(int val)
    {
        maxHealth += val;

        ChangeHealthBarSize();

        if (isMaxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = Mathf.Min(health, maxHealth);
        }
    }

    /// <summary>
    /// Heals the player for the given amount of health.
    /// This function also checks if the player has reached max health and sets the isMaxHealth variable accordingly.
    /// </summary>
    /// <param name="val">The amount of health to heal.</param>
    public new void Heal(int val)
    {
        base.Heal(val);

        if (health == maxHealth)
        {
            isMaxHealth = true;
        }
    }

    /// <summary>
    /// Change the size of the health bar.
    /// </summary>
    public void ChangeHealthBarSize()
    {
        healthSlider.transform.localScale = new Vector3(healthBarRatio * maxHealth, healthSlider.transform.localScale.y, healthSlider.transform.localScale.z);
        damageSlider.transform.localScale = new Vector3(healthBarRatio * maxHealth, damageSlider.transform.localScale.y, damageSlider.transform.localScale.z);

        healthSlider.maxValue = maxHealth;
        damageSlider.maxValue = maxHealth;
    }

    private IEnumerator ResetScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject grabber = GameObject.FindGameObjectWithTag("Grabber");
        string sceneName = SceneManager.GetActiveScene().name;

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        player.GetComponent<PlayerMovement>().stopMovement();
        grabber.GetComponent<Grabber>().goGrabbaGrabba = true;

        yield return new WaitForSecondsRealtime(2);

        TransitionManager.instance.LoadTransition(sceneName, CopyItemsAndRemoveInventory);
    }

    /// <summary>
    /// Remove the inventory system from the new scene after the player dies and copies over the old inventory.
    /// </summary>
    /// <returns>True when the code has finished running.</returns>
    private bool CopyItemsAndRemoveInventory(string sceneName)
    {
        // Find the inventory system and remove it from the scene
        GameObject inventory = GameObject.Find("InventorySystem");
        // Rename it so it doesn't get found again
        inventory.name = "InventorySystemOld";

        inventory = GameObject.Find("InventorySystem");
        Destroy(inventory);

        inventory = GameObject.Find("InventorySystemOld");
        inventory.name = "InventorySystem";

        // Move the inventory system to the new scene
        SceneManager.MoveGameObjectToScene(inventory, SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

        inventory.GetComponent<InventoryManager>().MoveToNewScene();

        return true;
    }

    private void SceneTransitionManager()
    {
        if (SceneManager.GetActiveScene().name.Contains("Boss"))
        {
            StartCoroutine(LoadGameOverScreen());
        }
        else
        {
            StartCoroutine(ResetScene());
        }
    }
}