using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    public GameObject explosionPrefab;
    public string loseSceneName = "LoseScene";

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        if (gameObject.CompareTag("Player"))
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in renderers) if (sr != null) sr.enabled = false;

            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (var col in colliders) if (col != null) col.enabled = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

            StartCoroutine(WaitAndChangeScene());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitAndChangeScene()
    {
        yield return new WaitForSecondsRealtime(1);

        if (Application.CanStreamedLevelBeLoaded(loseSceneName))
        {
            SceneManager.LoadScene(loseSceneName);
        }
    }
}