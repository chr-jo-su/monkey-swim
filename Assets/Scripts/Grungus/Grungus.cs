using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grungus : MonoBehaviour
{
    // set phase to 0 to make the boss phase out
    public bool appear = true;
    private float progress = 0;
    private Color visible = new Color(1f, 1f, 1f, 1f);
    private Color invisible = new Color(1f, 1f, 1f, 0f);
    private float timer = 0;
    private String direction = "left";
    public Animator animator;
    private bool gameOver = false;

    [SerializeField] private BossHealthBar bossHealth;

    void Start()
    {
        Mathf.Clamp(progress, 0, 1);
    }

    void Update()
    {
        if (bossHealth.GetHealth() <= 0 && !gameOver)
        {
            StartCoroutine(LoadNextLevel());
            gameOver = true;
        }

        if (appear)
        {
            progress += Time.deltaTime;
        }
        else
        {
            progress -= Time.deltaTime;
        }
        Phase();

        timer += Time.deltaTime;

        if (timer >= 3)
        {
            Attack();
            timer = 0;
        }
    }

    void Phase()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(invisible, visible, progress);
    }

    void Attack()
    {
        if (direction == "right")
        {
            animator.SetTrigger("RightAttack");
            direction = "left";
        }
        else
        {
            animator.SetTrigger("LeftAttack");
            direction = "right";
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bananarang(Clone)")
        {
            bossHealth.TakeDamage(BananarangDamage.instance.GetDamage());
            //if (!soundeffects)
            //    soundeffects = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();

            //int idx = Random.Range(0, painsounds.Length);
            //soundeffects.PlayOneShot(painsounds[idx]);

            //GetComponent<SpriteRenderer>().color = Color.red;
            //colorTimer = 0.1f;
        }
        else if (collision.gameObject.name == "Player")
        {
            PlayerHealthBar.instance.TakeDamage(25);
        }
    }

    private IEnumerator LoadNextLevel()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone) yield return null;

        InventoryManager.instance.beatBosses[1] = true;
        if (InventoryManager.instance.toWin())
        {
            TransitionManager.instance.LoadTransition("WinGame");
        }
        TransitionManager.instance.LoadTransition("Level2", CopyItemsAndRemoveInventory);
        // return null;
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
}
