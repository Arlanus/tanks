using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private int collisionCount = 0;
    private Rigidbody2D rb;
    private float lastCollisionTime = 0f;
    private float collisionCooldown = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.transform.root.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.gameObject.GetComponentInParent<Health>();
            if (enemyHealth == null) enemyHealth = collision.gameObject.GetComponentInChildren<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }

            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponentInParent<Health>();
            if (playerHealth == null) playerHealth = collision.gameObject.GetComponentInChildren<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            Destroy(gameObject);
            return;
        }

        if (Time.time - lastCollisionTime < collisionCooldown) return;
        lastCollisionTime = Time.time;

        collisionCount++;
        if (collisionCount >= 2)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (rb != null)
        {
            Vector2 currentVelocity = rb.linearVelocity;

            if (currentVelocity.sqrMagnitude > 0)
            {
                float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg - 90f;

                rb.MoveRotation(angle);
            }
        }
    }
}

